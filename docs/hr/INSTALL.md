# Instalacija dodatka Titlovi.com za Jellyfin 12

[English](../../INSTALL.md) · [Hrvatski](INSTALL.md) · [Српски](../sr-Cyrl/INSTALL.md)

> **Status izdanja:** Unified 2.0.0.0 za Jellyfin 12 još se priprema i nije dostupan u katalogu.

## Zahtjevi

- Jellyfin 12.x / .NET 10.
- Titlovi.com račun s API pristupom.
- Kompatibilan Jellyfin paket, ne Emby DLL ni GitHubova automatska arhiva izvornog koda.

## Instalacija iz kataloga

1. Na Jellyfinovoj nadzornoj ploči otvorite **Plugins → Repositories**.
2. Zadržite ili dodajte `https://titlovi.github.io/titlovi-jellyfin/repository.json`.
3. Nakon objave instalirajte **Titlovi.com** verziju **2.0.0.0** kompatibilnu s Jellyfinom 12.
4. Ponovno pokrenite Jellyfin, unesite korisničko ime i lozinku ako je potrebno, spremite postavke i provjerite pretraživanje i preuzimanje titla.

[Matkov izvorni video](https://github.com/user-attachments/assets/45e323d5-75c8-43b9-a525-fd705ace4235) prikazuje dodavanje repozitorija; izbornici Jellyfina 12 mogu izgledati drukčije.

## Nadogradnja izvornog Matkova dodatka

Izradite sigurnosnu kopiju podataka i konfiguracije poslužitelja, uključujući `plugins/configurations/Titlovi.Plugin.xml` ako postoji. Zadržite isti URL kataloga. Nasljednik zadržava izvorni GUID `6e25df50-638e-4109-a50b-03c14fc93fdd` i predviđen je za nadogradnju postojećeg dodatka, a ne paralelnu instalaciju. Ako u novoj konfiguraciji nema prijavnih podataka, jednokratno može preuzeti korisničko ime i lozinku iz izvorne konfiguracije. Stari token ne prenosi se, a izvorna XML datoteka ostaje netaknuta. Provjerite postavke, ponovno pokrenite poslužitelj te testirajte pretragu i preuzimanje.

**Nadogradnja Jellyfin poslužitelja s 10.11 na 12 zaseban je postupak.** Slijedite službene upute za migraciju poslužitelja i prije nadogradnje spremite bazu podataka. Povijesna izdanja 1.x ostaju dostupna za kompatibilne starije poslužitelje, ali ne treba ih instalirati na Jellyfin 12 ako nisu kompatibilna.

## Ručna instalacija i povratak na stariju verziju

Nakon objave preuzmite `titlovi-com_2.0.0.0.zip` s [Releases](https://github.com/Titlovi/titlovi-jellyfin/releases), zaustavite Jellyfin i izdvojite tri datoteke (`Jellyfin.Titlovi.dll`, `meta.json`, `thumb.png`) u zasebnu mapu unutar **aktivne** mape dodataka. Nemojte ostavljati dvostruke kopije istog dodatka. Ponovno pokrenite poslužitelj i provjerite rad.

Ako se dodatak ne učita, provjerite Jellyfinovu verziju, mapu dodataka i zapisnike. Vratite samo kompatibilnu prethodnu verziju i spremljenu konfiguraciju. Ne vraćajte bazu podataka Jellyfin poslužitelja na stariju verziju radi dodatka.

[Izvorni kod i podrška](https://github.com/Titlovi/titlovi-unified) · [Povratak na README](README.md)
