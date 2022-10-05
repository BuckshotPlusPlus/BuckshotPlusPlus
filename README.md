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

* [Getting Started](#getting-started)
* [Contributing](#contributing)
* [Creating A Pull Request](#creating-a-pull-request)
* [Authors](#authors)

## Getting Started

**Main.bpp**
```lua
include "Pages.bpp"
```

**Pages.bpp**
```lua
include "Pages/Home.bpp"

page index {
	title = "Home!"
	body = Home
}
```

**Pages/Home.bpp**
```lua
view Home {
   type = "body"
   margin = "0"
   padding = "0"

   content = "<h1>Home!</h1>"
}
```

## Authors

* **[Vic92548](https://github.com/Vic92548)** - *Developer* 
* **[MoskalykA](https://github.com/MoskalykA)** - *Developer* 
