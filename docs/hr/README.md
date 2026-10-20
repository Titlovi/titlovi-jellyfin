# Titlovi.com za Jellyfin

[English](../../README.md) · [Hrvatski](README.md) · [Српски](../sr-Cyrl/README.md)

[![Jellyfin 12](https://img.shields.io/badge/Jellyfin-12.x-0076bb)](INSTALL.md) [![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://github.com/Titlovi/titlovi-unified/tree/v2.0.0)
[![Latest release](https://img.shields.io/github/v/release/Titlovi/titlovi-jellyfin?label=release&color=0076bb)](https://github.com/Titlovi/titlovi-jellyfin/releases/latest)
[![ZIP preuzimanja (2.0)](https://img.shields.io/github/downloads/Titlovi/titlovi-jellyfin/v2.0.0/titlovi-com_2.0.0.0.zip?label=ZIP%20downloads%20(2.0)&color=4c1)](https://github.com/Titlovi/titlovi-jellyfin/releases)

**Pretražujte i preuzimajte titlove izravno u Jellyfinu 12.**

[Upute za instalaciju](INSTALL.md) · [Izvorni kod](https://github.com/Titlovi/titlovi-unified)

> **Objavljeno:** [Titlovi.com 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) dostupan je za **Jellyfin 12 / .NET 10** kroz postojeći katalog dodataka.

## Instalacija

Dodajte ovaj repozitorij u **Nadzorna ploča → Dodaci → Repozitoriji** (nazivi stavki mogu ovisiti o jeziku sučelja):

```text
https://titlovi.github.io/titlovi-jellyfin/repository.json
```

1. Dodajte navedeni URL repozitorija.
2. U katalogu pronađite **Titlovi.com** i odaberite verziju kompatibilnu s Jellyfinom 12 (2.0.0.0 za Jellyfin 12).
3. Ponovno pokrenite Jellyfin, konfigurirajte račun te pretražite i preuzmite titl.

### Video upute za instalaciju

https://github.com/user-attachments/assets/45e323d5-75c8-43b9-a525-fd705ace4235

*Izvorni Matkov video prikaz instalacije. Nazivi i raspored izbornika u Jellyfinu 12 mogu se razlikovati.*

## Konfiguracija

Potreban je Titlovi.com račun s API pristupom. U postavkama dodatka unesite **korisničko ime** i **lozinku**. Jellyfin sprema podatke lokalno u konfiguraciji poslužitelja. Dostupne su i opcije alternativnih jezika, pretvorbe u UTF-8 te dijagnostičkih zapisa.

## Mogućnosti

- Pretraživanje titlova za filmove i epizode serija.
- Uključivanje dodatnih rezultata na srpskom, hrvatskom i bosanskom jeziku.
- Rangiranje prema nazivu izdanja bez isključivanja ostalih valjanih rezultata.
- Obrada ZIP arhiva sezona/serija, ugniježđenih arhiva i više epizoda.
- Podrška za SRT, SUB, TXT, SSA i ASS.
- Neobavezna pretvorba preuzetih titlova u UTF-8 i dijagnostički zapisi.

Rezultati zadržavaju stvarni jezik koji vraća Titlovi.com, a različiti Titlovi medijski ID-ovi ostaju odvojeni.

## Zahtjevi i kompatibilnost

**Jellyfin 12.x / .NET 10** te Titlovi.com račun s API pristupom. [Jellyfin 2.0.0.0](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0) je objavljen. Izdanja 1.x ostaju dostupna za kompatibilne starije verzije Jellyfina; Unified 2.0 ne podržava Jellyfin 10.x.

**Nemojte instalirati GitHubovu automatsku arhivu „Source code (zip/tar.gz)”.** Upotrijebite katalog ili [službeni ZIP paket](https://github.com/Titlovi/titlovi-jellyfin/releases/tag/v2.0.0).

## Izvorni kod, podrška i starije verzije

Ovaj repozitorij služi za distribuciju Jellyfin dodatka i čuva postojeći katalog. Aktivni izvorni kod, razvoj i prijave problema nalaze se u [Titlovi/titlovi-unified](https://github.com/Titlovi/titlovi-unified). Matkov izvorni kod izdanja 1.x sačuvan je u [povijesnim oznakama](https://github.com/Titlovi/titlovi-jellyfin/tree/v1.2.5) i [starijim izdanjima](https://github.com/Titlovi/titlovi-jellyfin/releases). Licencija GPL-3.0; pogledajte [LICENSE](../../LICENSE) i [NOTICE](../../NOTICE.md).
