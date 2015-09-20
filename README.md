#Code Relay - Procedural Audio

We are taking it in turns to evolve a procedural soundscape. Join in and make some noise!

#Rules
- Every commit must change the audio output
- Commits should **add** no more than 100 lines of code
- Don't commit twice in a row. Let somebody else commit before coming back!

#Getting started
This C# project is setup for Visual Studio 2015 Community, but it should run on most C# platforms.

The program outputs a wav file at *"audio.wav"* by default but also takes the following command line arguments:

`-output <path>`  
Specify a new output path

`-exec <command>`
Command to execute after build, recommended to hook this up to your sound player

`-execargs <additional arguments>`  
Additional arguments to pass to the exec command
