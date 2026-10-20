# Titlovi.com за Jellyfin

[English](../../README.md) · [Hrvatski](../hr/README.md) · [Српски](README.md)

[![Jellyfin 12](https://img.shields.io/badge/Jellyfin-12.x-0076bb)](INSTALL.md) [![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://github.com/Titlovi/titlovi-unified/tree/v2.0.0)
[![Latest release](https://img.shields.io/github/v/release/Titlovi/titlovi-jellyfin?label=release&color=0076bb)](https://github.com/Titlovi/titlovi-jellyfin/releases/latest)
[![ZIP преузимања (2.0)](https://img.shields.io/github/downloads/Titlovi/titlovi-jellyfin/v2.0.0/titlovi-com_2.0.0.0.zip?label=ZIP%20downloads%20(2.0)&color=4c1)](https://github.com/Titlovi/titlovi-jellyfin/releases)

**Претражујте и преузимајте титлове директно у Jellyfin-у 12.**

[Упутство за инсталацију](INSTALL.md) · [Изворни код](https://github.com/Titlovi/titlovi-unified)

> **Објављено:** [Titlovi.com 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) доступан је за **Jellyfin 12 / .NET 10** преко постојећег каталога додатака.

## Инсталација

Додајте овај репозиторијум преко **Dashboard → Plugins → Repositories** (називи зависе од језика интерфејса):

```text
https://titlovi.github.io/titlovi-jellyfin/repository.json
```

1. Додајте наведени URL репозиторијума.
2. У каталогу пронађите **Titlovi.com** и изаберите верзију компатибилну са Jellyfin-ом 12 (2.0.0.0 за Jellyfin 12).
3. Поново покрените Jellyfin, подесите налог, па претражите и преузмите титл.

### Видео-упутство за инсталацију

https://github.com/user-attachments/assets/45e323d5-75c8-43b9-a525-fd705ace4235

*Оригинални Матков видео са приказом инсталације. Називи и распоред менија у Jellyfin-у 12 могу се разликовати.*

## Конфигурација

Потребан је Titlovi.com налог са API приступом. У подешавањима додатка унесите **корисничко име** и **лозинку**. Jellyfin их чува у локалној конфигурацији сервера. Постоје и опције алтернативних језика, претварања у UTF-8 и дијагностичког логовања.

## Могућности

- Претрага титлова за филмове и епизоде серија.
- Укључивање додатних резултата на српском, хрватском и босанском језику.
- Рангирање према називу издања, без искључивања других ваљаних резултата.
- Обрада ZIP архива сезона/серија, угнежђених архива и више епизода.
- Подршка за SRT, SUB, TXT, SSA и ASS.
- Опционо претварање преузетих титлова у UTF-8 и дијагностичко логовање.

Резултати чувају стварни језик који враћа Titlovi.com, а различити Titlovi медијски ID-ови остају одвојени.

## Захтеви и компатибилност

**Jellyfin 12.x / .NET 10** и Titlovi.com налог са API приступом. [Jellyfin 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) је објављен. Издања 1.x остају доступна за компатибилне старије верзије Jellyfin-а; Unified 2.0 не подржава Jellyfin 10.x.

**Не користите GitHub-ову аутоматску архиву „Source code (zip/tar.gz)” за инсталацију.** Користите каталог или [званични ZIP пакет](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0).

## Изворни код, подршка и старије верзије

Овај репозиторијум служи за дистрибуцију Jellyfin додатка и чува постојећи каталог. Активни изворни код, развој и пријаве проблема налазе се у [Titlovi/titlovi-unified](https://github.com/Titlovi/titlovi-unified). Матков изворни код издања 1.x сачуван је у [историјским ознакама](https://github.com/Titlovi/titlovi-jellyfin/tree/v1.2.5) и [старијим издањима](https://github.com/Titlovi/titlovi-jellyfin/releases). Лиценца GPL-3.0; погледајте [LICENSE](../../LICENSE) и [NOTICE](../../NOTICE.md).
