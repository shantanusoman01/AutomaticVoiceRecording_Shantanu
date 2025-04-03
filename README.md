# Microphone-Activated Recorder (.NET)

## Overview
This .NET console application automatically records audio when sound is detected from the microphone and stops recording when silence is detected for a set duration. It efficiently manages system resources by avoiding unnecessary recordings and saves the audio in `.wav` format.

## Features
- Auto Start: Begins recording when the microphone detects sound.
- Auto Stop: Stops recording after **3 seconds of silence**.
- Saves as WAV: Stores audio with timestamped filenames.
- Efficient: Avoids excessive resource usage.
- Command-Line Based: No UI needed.

## Requirements
- .NET SDK (Download from [here](https://dotnet.microsoft.com/download))
- NAudio Library (Install via NuGet)
