# jellyfin-titlovi

This plugin allows Jellyfin users to seamlessly access and download subtitles from Titlovi.com directly within the media server,
instead of manually searching for subtitle files. You can browse and fetch them through Jellyfin’s interface,
ensuring your movies and TV shows are always synced with high-quality, community-provided subtitles in multiple languages.

## installation

the plugin can be installed using a custom plugin repository. To add the repository, follow these steps:

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

*keep on mind, most plugin installs will require a restart of jellyfin!*
