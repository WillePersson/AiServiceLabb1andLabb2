# Multi-Function AI Service

This project is a multi-function AI service application that leverages Azure Cognitive Services to perform various tasks. The application currently supports two labs:

1. **Lab 1: Language Processing and QnA**
2. **Lab 2: Image Analysis**

## Table of Contents

- [Lab 1: Language Processing and QnA](#lab-1-language-processing-and-qna)
  - [Usage](#usage)
- [Lab 2: Image Analysis](#lab-2-image-analysis)
  - [Usage](#usage-1)

## Lab 1: Language Processing and QnA

### Usage

1. **Start the application** and select "Lab 1: Language Processing and QnA."
2. **Enter your question** when prompted. The question can be in any supported language.
3. The application will **detect the language** and **translate** the question if needed.
4. The application will then **query the QnA service** and display the **answer** along with the **confidence score**.
5. You can choose to ask another question or exit the lab.

## Lab 2: Image Analysis

### Usage

1. **Start the application** and select "Lab 2: Image Analysis."
2. **Enter the image file path or URL** when prompted.
3. **Specify the thumbnail dimensions** (width and height).
4. The application will **analyze the image** and display detected tags with confidence scores.
5. The application will then **generate a thumbnail** of the image and save it.
6. You will be asked if you want to **generate an image with bounding boxes**. If you choose yes, the image will be processed accordingly, and the output will be saved.
