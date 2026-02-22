# Histogram Contrast Corrector

Automatic image contrast equalization based on the histogram.

**Автоматическое выравнивания контраста изображения по гистограмме**

## Overview

Histogram Contrast Corrector is a Windows desktop application built with C# and Windows Forms that automatically enhances image contrast using histogram equalization techniques. The application provides an intuitive interface for processing and visualizing contrast adjustments.

## Features

- **Automatic Contrast Equalization**: Utilizes histogram equalization algorithms to automatically enhance image contrast
- **Multi-language Support**: Available in both English and Russian languages
- **Windows Forms UI**: User-friendly desktop application interface
- **GDAL Integration**: Leverages GDAL (Geospatial Data Abstraction Library) for advanced image processing

## Prerequisites

- .NET Framework (as configured in the project)
- GDAL libraries (MaxRev.Gdal.Core)
- Windows operating system

## Installation

1. Clone this repository:
```bash
git clone https://github.com/HC-NC/histogram-contrast-corrector.git
```

2. Open the solution in Visual Studio:
```bash
Histogram Contrast Corrector.sln
```

3. Build the project:
```bash
dotnet build
```

4. Run the application

## Usage

### Basic Usage

1. Launch the application
2. Load an image file
3. The contrast correction will be automatically applied based on histogram equalization
4. Save the processed image

### Language Settings

The application supports English and Russian languages. You can start the application with language arguments:

```bash
# English
Histogram Contrast Corrector.exe lang-en

# Russian
Histogram Contrast Corrector.exe lang-ru
```

## Project Structure

```
├── Program.cs                          # Main entry point
├── Forms/                              # Windows Forms UI components
├── DataClasses/                        # Data structures and models
├── Histogram Contrast Corrector.csproj # Project file
├── Histogram Contrast Corrector.sln    # Solution file
├── App.config                          # Application configuration
└── LICENSE                             # License information
```

## Technical Details

### Technologies Used

- **Language**: C# (.NET)
- **UI Framework**: Windows Forms
- **Image Processing**: GDAL (Geospatial Data Abstraction Library)
- **Target Platform**: Windows Desktop

### Key Components

- **Program.cs**: Contains the main entry point, initializes GDAL, and sets up the UI culture for multi-language support
- **WorkSpace**: Main application window (Windows Forms)
- **DataClasses**: Domain models for image processing operations
- **Forms**: UI components for the application interface

## How Histogram Equalization Works

Histogram equalization is a technique to adjust image intensities to enhance contrast:

1. Calculate the histogram of the input image
2. Compute the cumulative distribution function (CDF) of the histogram
3. Normalize the CDF to create a mapping function
4. Apply the mapping function to the original image pixels
5. The result is an image with improved contrast and detail visibility

This technique is particularly effective for images with poor contrast or uneven lighting.

## Configuration

Application settings can be configured through `App.config`. Ensure that GDAL is properly configured before running the application.

## Development

To contribute or modify the source code:

1. Open the solution file in Visual Studio 2019 or later
2. Make your changes
3. Build the solution to verify compilation
4. Test the application with various images

## License

This project is licensed under the terms specified in the LICENSE file. Please refer to [LICENSE](LICENSE) for more information.

## Support

For issues, questions, or suggestions, please open an issue on the repository.

## Author

Developed by HC-NC
