## 📏 Floating Ruler (WinForms)

A lightweight, always-on-top, rotatable on-screen ruler built with C# and Windows Forms.

This tool provides accurate centimeter measurements based on screen DPI and supports resizing, smooth rotation, and snap rotation.

## ✨ Features

📌 Always-on-top floating ruler

📏 Real centimeter measurement (DPI-based)

↔ Resizable from left and right edges

🔄 Smooth rotation using mouse wheel

🎯 Snap rotation (45° increments) using CTRL + Mouse Wheel

🖱 Drag to move

🖼 Clean anti-aliased rendering

🔲 Custom window region (no rectangular clipping)

🌗 Semi-transparent modern look

## 🎮 Controls
Action	Result
Drag center	Move ruler
Drag left/right edge	Resize ruler
Mouse Wheel	Rotate (2° precision)
CTRL + Mouse Wheel	Snap rotate (45° increments)
## 🛠 Technologies Used

C#

.NET WinForms

GDI+ Graphics

Custom Form Region

DPI-based measurement calculation

## 📐 How Measurement Works

The ruler calculates real-world centimeters using:

pixelsPerCm = Graphics.DpiX / 2.54

Because:

1 inch = 2.54 cm

DPI = dots (pixels) per inch

This ensures accurate scaling on most displays.

⚠ Note: Accuracy depends on correct system DPI settings.

## 🧠 Implementation Highlights

Uses Graphics.RotateTransform() for smooth rotation

Dynamically recalculates bounding box to prevent clipping

Updates Form.Region to match rotated ruler shape

Uses TextRenderingHint.ClearTypeGridFit for sharp number rendering

Double buffering enabled for flicker-free drawing

## 🚀 Getting Started

Create a new WinForms App

Replace Program.cs and RulerForm.cs with the provided source

Build and run

No external dependencies required.

## 🔮 Possible Future Improvements

Snap to screen edges

Click-through mode

Opacity slider

Save last position and angle

Vertical ruler mode

Inch / Pixel mode toggle

Two-point measurement tool

WPF GPU-accelerated version

## 📷 Preview

<img width="872" height="281" alt="image" src="https://github.com/user-attachments/assets/6bafce78-5515-493d-9676-6daf54857e55" />

<img width="607" height="664" alt="image" src="https://github.com/user-attachments/assets/af1fb208-43d2-4361-8985-f9a7d45511bc" />

## 📄 License

Free to use and modify.
