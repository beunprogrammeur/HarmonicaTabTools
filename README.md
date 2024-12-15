# HarmonicaTabTools

Hi there~ This repository is a test environment where I explore ideas to generate / convert etc. harmonica tablature.

It is currently a very early work in progress.

The supported harmonica's are:
 * C Richter
 * C Powerbender (lucky 13)

The biggest working feature at the moment is to convert the notes on guitar tablature to harmonica tablature.

Take this piece from Pat metheny's Last Train home (source can be found [here]())

```
e|---------------------------------------------------|
B|-----3---3-----2p1--------1------------------------|
G|-3--------------------3-------3----3---0---3-------|
D|---------------------------------------------------|
A|---------------------------------------------------|
E|---------------------------------------------------|
```

This can be converted to the following:
```
e|-------------------------------------------------------------------------------------------|
B|---------[-2'']---[-2'']-----[2]p[1o]------------[1o]--------------------------------------|
G|-[-1']-----------------------------------[-1']----------[-1']----[-1']---[X]---[-1']-------|
D|-------------------------------------------------------------------------------------------|
A|-------------------------------------------------------------------------------------------|
E|-------------------------------------------------------------------------------------------|
```
the notation:
|technique|notation|
|-|-|
|blow|5|
|blow half step bend|5'|
|blow full step bend|5''|
|overblow|5o|
|draw|-5|
|draw half step bend|-5'|
|draw full step bend|-5''|
|draw one and a half step bend|-5'''|
|overdraw|-5o|
|slider|5*|

## how to use?

The code is still in progress, but if you need it before I have finished some more stuff:

A short example could be:

```cs
string configPath = "path/to/Richter_C.xml";
var harmonicaTuningInC = TuningLoader.LoadTuningConfiguration(configPath);
var harmonicaTuningInA = TuningLoader.Transpose(harmonicaTuningInC, -3);

GuitarTuningConfiguration guitarTuning = new(StringConfiguration.Default);

TabbingConfiguration configuration = new()
{
    HarmonicaTuning = harmonicaTuningInA,
    GuitarTuning = guitarTuning
};

string inputFileContent = File.ReadAllText(inputFile);
string outputFileContent = GuitarTabConverter.ConvertGuitarTab(configuration, inputFileContent);
File.WriteAllText(outputFile, outputFileContent);
```


