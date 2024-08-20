# Multi-Function AI Service

This project is a multi-function AI service application that leverages Azure Cognitive Services to perform various tasks. The application currently supports two labs:

1. **Lab 1: Language Processing and QnA**
2. **Lab 2: Image Analysis**

## Table of Contents

- [Lab 1: Language Processing and QnA](#lab-1-language-processing-and-qna)
  - [Features](#features)
  - [How It Works](#how-it-works)
  - [Usage](#usage)
- [Lab 2: Image Analysis](#lab-2-image-analysis)
  - [Features](#features-1)
  - [How It Works](#how-it-works-1)
  - [Usage](#usage-1)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [Contributing](#contributing)
- [License](#license)

## Lab 1: Language Processing and QnA

### Features

- **Language Detection**: Detects the language of the user's input question.
- **Translation**: Automatically translates the user's question to English if it's not already in English.
- **QnA Service**: Queries an Azure QnA Maker knowledge base to find the most relevant answer to the user's question.
- **Confidence Scoring**: Displays the confidence score of the returned answer.
- **Interactive**: Allows the user to continue asking questions in any language.

### How It Works

1. **Language Detection**: When the user inputs a question, the application first detects the language using Azure's Text Analytics service.
2. **Translation**: If the detected language is not English, the question is translated into English using Azure's Translator service.
3. **QnA Query**: The translated question is then sent to an Azure QnA Maker service, which returns the best possible answer based on the trained knowledge base.
4. **Answer Display**: The application displays the most relevant answer along with its confidence score.
5. **User Interaction**: The user is prompted to ask another question or exit the lab.

### Usage

1. **Start the application** and select "Lab 1: Language Processing and QnA."
2. **Enter your question** when prompted. The question can be in any supported language.
3. The application will **detect the language** and **translate** the question if needed.
4. The application will then **query the QnA service** and display the **answer** along with the **confidence score**.
5. You can choose to ask another question or exit the lab.

## Lab 2: Image Analysis

### Features

- **Image Analysis**: Detects and tags objects within an image provided via a local file path or URL.
- **Thumbnail Generation**: Creates a thumbnail of the image with user-specified dimensions.
- **Bounding Boxes**: Optionally draws bounding boxes around detected objects in the image.
- **Support for URLs**: The application can analyze images from URLs, downloading them if necessary.

### How It Works

1. **Image Input**: The user inputs a local file path or URL of an image they want to analyze.
2. **Image Analysis**: The application uses Azure's Computer Vision service to analyze the image and identify objects, generating tags with confidence scores.
3. **Thumbnail Generation**: The user can specify the dimensions, and a thumbnail of the image is generated and saved.
4. **Bounding Boxes**: The user can opt to create a version of the image with bounding boxes drawn around detected objects. This image is also saved locally.

### Usage

1. **Start the application** and select "Lab 2: Image Analysis."
2. **Enter the image file path or URL** when prompted.
3. **Specify the thumbnail dimensions** (width and height).
4. The application will **analyze the image** and display detected tags with confidence scores.
5. The application will then **generate a thumbnail** of the image and save it.
6. You will be asked if you want to **generate an image with bounding boxes**. If you choose yes, the image will be processed accordingly, and the output will be saved.
