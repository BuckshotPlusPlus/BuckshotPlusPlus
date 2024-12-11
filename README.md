<br/>
<p align="center">
  <a href="https://github.com/BuckshotPlusPlus/BuckshotPlusPlus">
    <img src="https://i.imgur.com/MvYk4FB.png" alt="Logo">
  </a>

  <p align="center">
    <strong>A modern, simple, and efficient web development language</strong>
    <br/>
    <br/>
    <a href="https://bpp.gitbook.io/buckshot++/"><strong>📚 Documentation</strong></a>
    ·
    <a href="https://github.com/BuckshotPlusPlus/Official-Website">🎯 Demo</a>
    ·
    <a href="https://github.com/BuckshotPlusPlus/BuckshotPlusPlus/issues">🐛 Report Bug</a>
    ·
    <a href="https://github.com/BuckshotPlusPlus/BuckshotPlusPlus/pulls">✨ Request Feature</a>
  </p>
</p>

![Downloads](https://img.shields.io/github/downloads/BuckshotPlusPlus/BuckshotPlusPlus/total) ![Contributors](https://img.shields.io/github/contributors/BuckshotPlusPlus/BuckshotPlusPlus?color=dark-green) ![Forks](https://img.shields.io/github/forks/BuckshotPlusPlus/BuckshotPlusPlus?style=social) ![Stargazers](https://img.shields.io/github/stars/BuckshotPlusPlus/BuckshotPlusPlus?style=social) ![Issues](https://img.shields.io/github/issues/BuckshotPlusPlus/BuckshotPlusPlus) ![License](https://img.shields.io/github/license/BuckshotPlusPlus/BuckshotPlusPlus)

## 🚀 About BuckshotPlusPlus

BuckshotPlusPlus (BPP) is a modern web development language designed to make web development simpler and more efficient. It combines the power of component-based architecture with a clean, intuitive syntax to help you build websites faster.

## ✨ Features

- 🎨 **Component-Based Architecture** - Create reusable views and layouts
- 🎯 **Simple Event Handling** - Handle user interactions with ease
- 🌐 **Integrated Server** - Built-in development server included
- 🔌 **API Integration** - Connect to external services seamlessly

## 💡 Why BuckshotPlusPlus?

BPP is not just another frontend framework - it's a complete fullstack solution. With BPP, you can:

- Build complete web applications with a single language
- Handle both frontend and backend logic seamlessly
- Integrate with external APIs and databases effortlessly
- Create dynamic, data-driven applications
- Deploy as static sites or full web applications

## 🛠️ Installation

### NPM Installation (Recommended)
```bash
npm install -g buckshotplusplus
```

### Manual Installation
Download the latest executable from the [releases section](https://github.com/BuckshotPlusPlus/BuckshotPlusPlus/releases).

## 💻 Examples

### Working with External Data
A racing quote generator using the Kimi Räikkönen API:

```lua
source KimiQuote {
    url = "https://kimiquotes.pages.dev/api/quote"
    method = "GET"
}

view QuoteText {
  type = "p"
  content = KimiQuote.quote
  font-size = "24px"
  margin = "24px"
  text-align = "center"
  max-width = "600px"
  font-style = "italic"
}

view RefreshButton {
  type = "button"
  content = "Bwoah, Get Another Quote"
  padding = "12px 24px"
  background = "#3B82F6"
  color = "white"
  border = "none"
  border-radius = "8px"
  cursor = "pointer"
  onclick = GetNewQuote
}

view QuoteApp {
  type = "div"
  content = [QuoteText, RefreshButton]
  display = "flex"
  flex-direction = "column"
  align-items = "center"
  padding = "24px"
}

event GetNewQuote {
  QuoteText.content = KimiQuote.quote
}

page index {
  title = "Kimi Quotes"
  body = QuoteApp
}
```

Each example above is fully functional and demonstrates different aspects of BPP:
- Component composition
- Style management
- Event handling
- Data management
- External API integration
- State updates

## 📖 Quick Start

1. Create a new file `main.bpp`:

```lua
view Title {
  content = "Hello World!"
  type = "h1"
  color = "blue"
}

view Home {
  type = "body"
  margin = "0"
  padding = "0"
  content = Title
}

page index {
  title = "Home!"
  body = Home
}
```

2. Start the development server:

Using NPM installation:
```bash
bpp main.bpp
```

Using executable:
```bash
BuckshotPlusPlus.exe "Path/To/Main.bpp"
```

3. Visit `http://localhost:8080` in your browser

## 🔧 Commands

- `bpp <file>` - Run a BPP file
- `bpp export <file> <dir>` - Export your website to static files
- `bpp merge <file>` - Merge all includes into a single file
- `bpp -h` - Show help message
- `bpp --version` - Show version information

## 📚 Documentation

Visit our [comprehensive documentation](https://doc.bpplang.com) to learn more about:

- Views and Components
- Data Management
- Event Handling
- API Integration
- Styling
- Best Practices

## 🌟 Examples

Check out these example projects to learn more:

- [Official Website](https://github.com/BuckshotPlusPlus/Official-Website)
- [KimiSays](https://github.com/Vic92548/KimiSays) - A quote generator showcasing API integration

## 🤝 Contributing

Contributions are what make the open source community amazing! Feel free to:

1. Fork the Project
2. Create your Feature Branch
3. Commit your Changes
4. Push to the Branch
5. Open a Pull Request

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

## 👥 Authors

- **[Vic92548](https://github.com/Vic92548)** - _Lead Developer_
- **[MoskalykA](https://github.com/MoskalykA)** - _Developer_

## 📄 License

BuckshotPlusPlus is licensed under the MIT License. See [LICENSE](LICENSE) for more information.

## 💫 Acknowledgments

- Thanks to all our contributors
- Special thanks to our amazing community
- Inspired by modern web development frameworks

---
<p align="center">Made with ❤️ by Victor Chanet and the BuckShotPlusPlus community</p>