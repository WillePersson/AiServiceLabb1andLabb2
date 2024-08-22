# AI Service Labbs

This project contains two distinct labs, each demonstrating different aspects of AI services using Azure Cognitive Services. Below is a brief description of each labb.

## Labb 1: Language Processing and QnA

### Overview
Labb 1 showcases the use of Natural Language Processing (NLP) combined with Azure's QnA Maker service. The primary focus is to enable users to ask questions in any language, which are then translated to English and answered using a pre-trained knowledge base.

### Key Features
- **Language Detection**: Automatically detects the language of the input question.
- **Translation**: Translates non-English questions into English.
- **QnA Service**: Queries a QnA Maker knowledge base to provide the most relevant answer.
- **Interactive QnA**: Users can continuously ask questions and receive answers until they choose to exit.

### Usage
The user is prompted to enter a question, which can be in any supported language. The system detects the language, translates the question to English if necessary, and then queries the QnA Maker service for an answer. The detected language, translated question, and the answer (with a confidence score) are displayed.

## Labb 2: Image Analysis

### Overview
Labb 2 demonstrates the capabilities of Azure's Computer Vision service by analyzing images. Users can provide a local image file or a URL to analyze its content, generate a thumbnail, and optionally, create an image with bounding boxes around detected objects.

### Key Features
- **Image Analysis**: Detects objects and tags within an image, providing confidence scores.
- **Thumbnail Generation**: Creates a thumbnail of the image with user-specified dimensions.
- **Bounding Boxes**: Optionally generates an image with bounding boxes drawn around detected objects.
- **Image Paths**: After processing, the file paths of the saved images are displayed to the user.

### Usage
The user is prompted to enter an image path or URL, followed by the desired dimensions for the thumbnail. The system analyzes the image, generates a thumbnail, and optionally creates an image with bounding boxes. The paths to the generated images are displayed to the user for easy access.
