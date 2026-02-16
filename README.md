# titlovi-jellyfin

![Dynamic YAML Badge](https://img.shields.io/badge/dynamic/yaml?url=https%3A%2F%2Fraw.githubusercontent.com%2Ftitlovi%2Ftitlovi-jellyfin%2Frefs%2Fheads%2Fmain%2Fbuild.yaml&query=targetAbi&label=Jellyfin%20ABI)
![Dynamic YAML Badge](https://img.shields.io/badge/dynamic/yaml?url=https%3A%2F%2Fraw.githubusercontent.com%2Ftitlovi%2Ftitlovi-jellyfin%2Frefs%2Fheads%2Fmain%2Fbuild.yaml&query=framework&label=.NET%20Framework)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/Titlovi/titlovi-jellyfin/total)

This plugin allows Jellyfin users to seamlessly access and download subtitles from Titlovi.com directly within the media server,
instead of manually searching for subtitle files. You can browse and fetch them through Jellyfin’s interface,
ensuring your movies and TV shows are always synced with high-quality, community-provided subtitles in multiple languages.

## usage

**IMPORTANT**

To use the plugin you must enable api access on [Titlovi](https://titlovi.com/clanci/8871/podrzi-titlove-please/?debug=1). When you first register you
get 30 days free, after that you must have atleast a **"zmaj supporter"** subscription active.

## installation

### repository
* name: Titlovi
* url: https://titlovi.github.io/titlovi-jellyfin/repository.json

### video

https://github.com/user-attachments/assets/45e323d5-75c8-43b9-a525-fd705ace4235

## configuration

before the plugin can be used you will have to configure it. you will need to provide
the following information in the configuration page, for the plugin to function correctly.
(*your authentication information is only stored on the local jellyfin server via its configuration system.*)

| Property | Description                                                                               |
| -------- | ----------------------------------------------------------------------------------------- |
| Username | The username used to authenticate with titlovi.com                                        |
| Password | The password used to authenticate with titlovi.com                                        |
