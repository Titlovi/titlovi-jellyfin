# jellyfin-titlovi

![Dynamic YAML Badge](https://img.shields.io/badge/dynamic/yaml?url=https%3A%2F%2Fraw.githubusercontent.com%2Fmaatko%2Fjellyfin-titlovi%2Frefs%2Fheads%2Fmain%2Fbuild.yaml&query=targetAbi&label=Jellyfin%20ABI)
![Dynamic YAML Badge](https://img.shields.io/badge/dynamic/yaml?url=https%3A%2F%2Fraw.githubusercontent.com%2Fmaatko%2Fjellyfin-titlovi%2Frefs%2Fheads%2Fmain%2Fbuild.yaml&query=framework&label=.NET%20Framework)

This plugin allows Jellyfin users to seamlessly access and download subtitles from Titlovi.com directly within the media server,
instead of manually searching for subtitle files. You can browse and fetch them through Jellyfin’s interface,
ensuring your movies and TV shows are always synced with high-quality, community-provided subtitles in multiple languages.

## installation

the plugin can be installed through a custom plugin repository.

### repository

1. open your admin dashboard and navigate to `Plugins`
2. open the `Catalog` page
3. at the top of the page click the gear symbol for settings
4. next click the `+` button to add a new repository
5. fill out the required information with following
   - **Repository Name** = `Titlovi`
   - **Repository Url** = `https://maatko.github.io/jellyfin-titlovi/repository.json`
6. finally click the save button

### plugin

1. add the repository with the steps above
2. open your admin dashboard and navigate to `Plugins`
3. open the `Catalog` page
4. from there find the `Titlovi.com` plugin and click it
5. click the `Install` button

**keep on mind, most plugin installs will require a restart of jellyfin!**

## configuration

before the plugin can be used you will have to configure it. you will need to provide
the following information in the configuration page, for the plugin to function correctly.
(*your authentication information is only stored on the local jellyfin server via its configuration system.*)

| Property | Description                                                                               |
| -------- | ----------------------------------------------------------------------------------------- |
| Username | The username used to authenticate with titlovi.com                                        |
| Password | The password used to authenticate with titlovi.com                                        |

**rate limits might apply if you dont have don't have a "zmaj supporter" subscription active**
