# NuGet packages

The `base` folder contains .nuspec files that contain variables for $(FsVersion) and $(FsConfiguration).

The packall.bat script creates a copy of this folder, replaces those variables and creates the NuGet packages.

This is handy for development, but after v1.0 is released I'd imagine we'd want to version each project separately?

## Dependencies

The packall.bat script requires two exes in this folder:

nuget.exe - www.nuget.org/downloads
fart.exe - fart-it.sourceforge.net

## Parameters

The packall.bat script requires a version number and configuration variable to be set.

These are automatically set using version.txt and configuration.txt respectively. If the files do not exist, it'll ask for user input.