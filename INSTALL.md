# Installing Titlovi.com on Jellyfin 12

[English](INSTALL.md) · [Hrvatski](docs/hr/INSTALL.md) · [Српски](docs/sr-Cyrl/INSTALL.md)

> **Published:** [Unified 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) is available in the existing Jellyfin catalogue for **Jellyfin 12**.

## Requirements

- Jellyfin 12.x / .NET 10.
- A Titlovi.com account.
- The compatible Jellyfin package; not the Emby DLL or GitHub's auto-generated source archive.

## Install from the catalogue

1. In the Jellyfin dashboard open **Plugins → Repositories**.
2. Keep or add `https://titlovi.github.io/titlovi-jellyfin/repository.json`.
3. Install **Titlovi.com** version **2.0.0.0** for Jellyfin 12.
4. Restart Jellyfin; enter your Titlovi.com username and password if needed, save settings and test a movie or episode subtitle download.

The [original installation video](https://github.com/user-attachments/assets/45e323d5-75c8-43b9-a525-fd705ace4235) by Matko demonstrates adding the repository; menus may look different on Jellyfin 12.

## Upgrade from the original Matko plugin

Back up server data and configuration, including `plugins/configurations/Titlovi.Plugin.xml` if present. Keep the same catalogue URL. The successor retains the original plugin GUID `6e25df50-638e-4109-a50b-03c14fc93fdd` and is intended to upgrade the existing plugin rather than install alongside it. With no credentials configured in the new plugin, it can import username and password from the original configuration once. The old token is not reused; the original XML remains untouched. Verify settings, restart, search and download.

**Upgrading the Jellyfin server from 10.11 to 12 is a separate process.** Follow Jellyfin's official server migration instructions and back up the server database before that upgrade. Older 1.x packages remain available for compatible older Jellyfin versions. Matko's original 1.x plugin is not compatible with Jellyfin 12.

## Manual installation and rollback

Download `titlovi-com_2.0.0.0.zip` from the [official release](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0). Stop Jellyfin, back up the existing plugin folder and configuration, **move the old plugin folder outside the active plugins directory**, and extract its three files (`Jellyfin.Titlovi.dll`, `meta.json`, `thumb.png`) into a dedicated folder under the **active** Jellyfin plugins directory. Do not leave duplicate copies of the same plugin. Restart and verify.

If a plugin fails to load, check the Jellyfin version, plugin directory and logs. Restore only a version compatible with your server, along with backed-up configuration. Never downgrade a Jellyfin server database as a plugin rollback.

[Source and support](https://github.com/Titlovi/titlovi-unified) · [Back to README](README.md)
