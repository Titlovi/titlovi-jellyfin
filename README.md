# Titlovi.com for Jellyfin

[English](README.md) · [Hrvatski](docs/hr/README.md) · [Српски](docs/sr-Cyrl/README.md)

[![Jellyfin 12](https://img.shields.io/badge/Jellyfin-12.x-0076bb)](INSTALL.md) [![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://github.com/Titlovi/titlovi-unified/tree/v2.0.0)
[![Latest release](https://img.shields.io/github/v/release/Titlovi/titlovi-jellyfin?label=release&color=0076bb)](https://github.com/Titlovi/titlovi-jellyfin/releases/latest)
[![ZIP downloads (2.0)](https://img.shields.io/github/downloads/Titlovi/titlovi-jellyfin/v2.0.0/titlovi-com_2.0.0.0.zip?label=ZIP%20downloads%20(2.0)&color=4c1)](https://github.com/Titlovi/titlovi-jellyfin/releases)

**Search and download subtitles directly in Jellyfin 12.**

[Installation guide](INSTALL.md) · [Source code](https://github.com/Titlovi/titlovi-unified)

> **Released:** [Titlovi.com 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) is available for **Jellyfin 12 / .NET 10** through the existing plugin catalogue.

## Installation

Add this plugin repository under **Dashboard → Plugins → Repositories**:

```text
https://titlovi.github.io/titlovi-jellyfin/repository.json
```

1. Add the repository URL shown above.
2. Find **Titlovi.com** in the catalogue and select the version compatible with Jellyfin 12 (2.0.0.0 for Jellyfin 12).
3. Restart Jellyfin, configure your account, then search for and download a subtitle.

### Video installation guide

https://github.com/user-attachments/assets/45e323d5-75c8-43b9-a525-fd705ace4235

*Original installation walkthrough by Matko. Jellyfin 12 menu labels may differ from those in the recording.*

## Configuration

A Titlovi.com account is required. Enter your **username** and **password** in the plugin configuration. Jellyfin stores the credentials in its local server configuration. Optional settings include alternative subtitle languages, UTF-8 conversion and diagnostic logging.

## Features

- Search for movie and television-episode subtitles.
- Optionally include Serbian, Croatian and Bosnian alternative-language results.
- Rank release-name matches without excluding other valid results.
- Handle season/series ZIP files, nested archives and grouped episodes.
- Support SRT, SUB, TXT, SSA and ASS formats.
- Optionally convert downloaded subtitles to UTF-8 and enable diagnostic logging.

Search results preserve the language returned by Titlovi.com, and different Titlovi media IDs remain distinct.

## Requirements and compatibility

**Jellyfin 12.x / .NET 10** and a Titlovi.com account with API access. [Jellyfin 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) is published. Original 1.x releases remain available for compatible older Jellyfin versions; Unified 2.0 is not compatible with Jellyfin 10.x.

**Do not install GitHub’s automatically generated “Source code (zip/tar.gz)” archive.** Install through the catalogue or use the [official Jellyfin 2.0 ZIP](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0).

## Source, support and legacy versions

This repository distributes Jellyfin packages and maintains the existing catalogue. Active source code, development and issue tracking live in [Titlovi/titlovi-unified](https://github.com/Titlovi/titlovi-unified). Matko's original Jellyfin 1.x source is preserved under [historical source tags](https://github.com/Titlovi/titlovi-jellyfin/tree/v1.2.5) and [earlier releases](https://github.com/Titlovi/titlovi-jellyfin/releases). GPL-3.0; see [LICENSE](LICENSE) and [NOTICE](NOTICE.md).
