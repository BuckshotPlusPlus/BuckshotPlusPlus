<br/>
<p align="center">
  <a href="https://github.com/BuckshotPlusPlus/BuckshotPlusPlus">
    <img src="https://i.imgur.com/MvYk4FB.png" alt="Logo">
  </a>

  <p align="center">
    <a href="https://victor-chanet.gitbook.io/buckshot++/"><strong>Explore the docs</strong></a>
    <br/>
    <a href="https://github.com/BuckshotPlusPlus/Official-Website">View Demo</a>
  -
    <a href="https://github.com/BuckshotPlusPlus/BuckshotPlusPlus/issues">Report Bug</a>
  -
    <a href="https://github.com/BuckshotPlusPlus/BuckshotPlusPlus/pulls">Request Feature</a>
  </p>
</p>

![Downloads](https://img.shields.io/github/downloads/BuckshotPlusPlus/BuckshotPlusPlus/total) ![Contributors](https://img.shields.io/github/contributors/BuckshotPlusPlus/BuckshotPlusPlus?color=dark-green) ![Forks](https://img.shields.io/github/forks/BuckshotPlusPlus/BuckshotPlusPlus?style=social) ![Stargazers](https://img.shields.io/github/stars/BuckshotPlusPlus/BuckshotPlusPlus?style=social) ![Issues](https://img.shields.io/github/issues/BuckshotPlusPlus/BuckshotPlusPlus) ![License](https://img.shields.io/github/license/BuckshotPlusPlus/BuckshotPlusPlus)

## Table Of Contents

- [Getting Started](#getting-started)
- [Contributing](CONTRIBUTING.md)
- [Authors](#authors)

## Getting Started

Get the lastest executable for Buckshot++ in teh release section or compile from source.

Create a main.bpp

**Main.bpp**

The example code below will create a simple buckshgot++ website with an index page displaying the title "Hello World!".

```lua
view Title{
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

**Start your BuckShot++ server**

```shell
BuchshotPlusPlus.exe "Path/To/Main.bpp"
```
Enjoy!

## Authors

- **[Vic92548](https://github.com/Vic92548)** - _Lead Developer_
- **[MoskalykA](https://github.com/MoskalykA)** - _Developer_
