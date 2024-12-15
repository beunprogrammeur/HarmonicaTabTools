using Harmonica.Core.Midi;
using Harmonica.Core.Tuning;
using Harmonica.GuitarTabConverter.Core.Guitar;
using System.Text;
using System.Text.RegularExpressions;

namespace Harmonica.GuitarTabConverter.Core
{
    public static partial class GuitarTabConverter
    {
        public static string ConvertGuitarTab(TabbingConfiguration configuration, string tab)
        {
            StringBuilder stringBuilder = new();
            using StringReader stringReader = new(tab);

            Dictionary<GuitarString, string?> tabLines = [];

            foreach (GuitarString @string in configuration.StringOrder)
            {
                tabLines[@string] = null;
            }

            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (!IsGuitarTabLineRegex().IsMatch(line))
                {
                    // TODO: check for tab continuations
                    // lines that continue the tab because it didn't fit the screen etc.


                    stringBuilder.AppendLine(line); 
                    continue;
                }

                foreach(GuitarString @string in configuration.StringOrder)
                {
                    if (tabLines[@string] == null)
                    {
                        tabLines[@string] = line;
                        break;
                    }
                }


                if(!tabLines.Values.Any(x => x == null))
                {
                    HandleGuitarNeckTab(configuration, tabLines, stringBuilder);

                    // reset for the next tabbed neck
                    foreach (GuitarString guitarString in configuration.StringOrder)
                    {
                        tabLines[guitarString] = null;
                    }
                }



                //HandleSingleGuitarTabLine(stringBuilder, line);
            }

            return stringBuilder.ToString();
        }

        private static void HandleGuitarNeckTab(TabbingConfiguration configuration, Dictionary<GuitarString, string> tabLines, StringBuilder output)
        {
            Dictionary<GuitarString, StringBuilder> outputTabLines = [];
            
            foreach (GuitarString guitarString in configuration.StringOrder)
            {
                outputTabLines[guitarString] = new StringBuilder();
            }
            
            int maxLength = tabLines.Values.Max(x => x.Length);
            for (int characterIndex = 0; characterIndex < maxLength; characterIndex++)
            {
                foreach (GuitarString guitarString in configuration.StringOrder)
                {
                    string @string = tabLines[guitarString];
                    if (string.IsNullOrEmpty(@string) || @string.Length < characterIndex)
                    {
                        continue;
                    }

                    if (GetFret(@string, characterIndex, out int fret))
                    {
                        int stringPitch = configuration.GetPitchForGuitarString(guitarString);
                        string harmonicaNotation = GetHarmonicaEquivalentString(configuration, stringPitch, fret);
                        outputTabLines[guitarString].Append(harmonicaNotation);
                    }
                    else
                    {
                        // blindly add character to output
                        outputTabLines[guitarString].Append(@string[characterIndex]);
                    }
                }

                // to even out all strings again, so it reads the same
                // as it did before we inserted more characters
                int maxCurrentOutputLength = outputTabLines.Max(x => x.Value.Length);
                foreach(GuitarString guitarString in configuration.StringOrder)
                {
                    StringBuilder stringBuilder = outputTabLines[guitarString];
                    while(stringBuilder.Length < maxCurrentOutputLength)
                    {
                        stringBuilder.Append('-');
                    }
                }
            }


            foreach(GuitarString guitarString in configuration.StringOrder)
            {
                output.AppendLine(outputTabLines[guitarString].ToString());
            }
        }

        private static bool GetFret(string line, int index, out int fret)
        {
            fret = 0;
            
            if (index < 1)
            {
                return false;
            }

            if (!char.IsDigit(line[index]))
            {
                return false;
            }

            // if the previous index is a digit, we already have handled this one
            // so: fret 12
            //           ^ current index
            // we already have seen 1,2, so if we do this again, we'd get 12 and 2
            if (char.IsDigit(line[index - 1]))
            {
                return false;
            }

            if (line.Length >= index + 1 && char.IsDigit(line[index + 1]))
            {
                // fret is 10 or higher
                return int.TryParse(line.AsSpan(index, 2), out fret);
            }

            // fret is 0-9
            return int.TryParse(line.AsSpan(index, 1), out fret);
        }


        [Obsolete]
        private static void HandleSingleGuitarTabLine(TabbingConfiguration configuration, StringBuilder builder, string line)
        {
            GuitarString guitarString = GetGuitarStringForTabLine(line);
            
            // the line is not part of the guitar tab. add to the collection as text
            if (guitarString == GuitarString.Unknown)
            {
                builder.AppendLine(line);
                return;
            }

            int stringPitch = configuration.GuitarTuning.NotesForStrings[guitarString].MidiPitch;


            Regex regex = GuitarTabFretRegex();
            string newLine = regex.Replace(line, match => GetHarmonicaEquivalentString(configuration, stringPitch, match));
            builder.AppendLine(newLine);
        }

        private static string GetHarmonicaEquivalentString(TabbingConfiguration configuration, int stringPitch, int fret)
        {
            int fretPitch = stringPitch + fret;

            foreach ((string identifier, HarmonicaHoleConfiguration hole) in configuration.HarmonicaTuning.Holes)
            {
                foreach (PlayingTechnique technique in configuration.AllowedHarmonicaTechniques)
                {
                    if (!hole.SupportedTechniques.Contains(technique))
                    {
                        continue;
                    }

                    Note note = hole[technique];
                    if (note.MidiPitch == fretPitch)
                    {
                        return DecorateHarmonicaNoteString(configuration, identifier, technique);
                    }
                }
            }

            return $"[{configuration.UnknownNoteCharacter}]";
        }
        private static string GetHarmonicaEquivalentString(TabbingConfiguration configuration, int stringPitch, Match match)
        {
            if(!int.TryParse(match.Value, out int fret))
            {
                return "?";
            }

            return GetHarmonicaEquivalentString(configuration, stringPitch, fret);
        }

        private static string DecorateHarmonicaNoteString(TabbingConfiguration configuration, string identifier, PlayingTechnique technique) => technique switch
        {
            PlayingTechnique.Draw => $"[-{identifier}]",
            PlayingTechnique.Blow => $"[{identifier}]",
            PlayingTechnique.DrawBend1 => $"[-{identifier}']",
            PlayingTechnique.DrawBend2 => $"[-{identifier}'']",
            PlayingTechnique.DrawBend3 => $"[-{identifier}''']",
            PlayingTechnique.OverDraw => $"[-{identifier}o]",
            PlayingTechnique.BlowBend1 => $"[{identifier}']",
            PlayingTechnique.BlowBend2 => $"[{identifier}'']",
            PlayingTechnique.OverBlow => $"[{identifier}o]",
            _ => $"[{configuration.UnknownNoteCharacter}]",
        };

        [Obsolete]
        private static GuitarString GetGuitarStringForTabLine(string line)
        {
            Regex regex = GuitarTabStringRegex();
            Match match = regex.Match(line);
            if (!match.Success)
            {
                return GuitarString.Unknown;
            }

            return match.Groups[1].Value switch
            {
                "E" => GuitarString.LowE,
                "A" => GuitarString.A,
                "D" => GuitarString.D,
                "G" => GuitarString.G,
                "B" => GuitarString.B,
                "e" => GuitarString.HighE,
                _ => throw new NotImplementedException("unknown string")
            };
        }

        [GeneratedRegex(@"^\s*?(\w)\|", RegexOptions.Compiled)]
        private static partial Regex GuitarTabStringRegex();

        // TODO: Can be improved upon so we don't grab numbers
        // that are like comments at the right hand side of the tab's line
        [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
        private static partial Regex GuitarTabFretRegex();

        [GeneratedRegex(@"^\w\|.*\|", RegexOptions.Compiled)]
        private static partial Regex IsGuitarTabLineRegex();
    }
}
