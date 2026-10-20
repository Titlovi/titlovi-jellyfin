# Jellyfin 2.0.0.0 release preparation — internal checklist

> **Historical preparation record.** Titlovi.com 2.0.0.0 for Jellyfin 12 was published on 2026-10-20 and added to the public catalogue. The catalogue-based upgrade from the original plugin was successfully tested on Jellyfin 12. Earlier references to a draft PR, pending publication, or untested catalogue upgrades are retained only as the pre-release record. For current instructions see [README](README.md) and [INSTALL](INSTALL.md).


**NOT FOR PUBLICATION**. Review-only. Do not merge PR #7, create a public release, or update `gh-pages` without approval.

## Verified

- Immutable source: `Titlovi/titlovi-unified` tag `v2.0.0`, commit `1c4c6dfd04fc0bf51b88a1c8e276896e5f0cab78`.
- Workflow #4: https://github.com/Titlovi/titlovi-unified/actions/runs/39495630298
- Windows regressions **166 passed**; Linux regressions **166 passed**.
- Windows and Linux: Emby and Jellyfin builds succeeded, candidate ZIP and metadata checks succeeded.
- Original catalogue contained **nine historical 1.x versions** and original GUID `6e25df50-638e-4109-a50b-03c14fc93fdd`.
- User's real Jellyfin 12 test: load PASS, configuration PASS, credentials PASS, movie subtitle search/download PASS, episode search/download PASS.
- Draft distribution conversion: PR #7, no merge.
- Release notes in `RELEASE-NOTES-DRAFT.md` (three languages).

## Publication sequence (approval required)

1. Review and merge distribution PR #7 **before** publishing the new Jellyfin release, disabling the old `publish.yaml` workflow. Preserve `LICENSE`, `logo.png`, all old Git history, tags, release assets and the entire `gh-pages` branch.
2. Verify the published target source in Unified `v2.0.0`. Confirm final release asset is the already-tested ZIP from the approved CI run. Do **not** rebuild silently or substitute a different ZIP without repeating required checks.
3. Prepare tag `v2.0.0` **in Jellyfin distribution repository** pointing to the distribution-only snapshot after merge. Do not confuse it with the **existing Unified source tag** of the same name.
4. Publish Jellyfin release **`Titlovi.com 2.0.0.0 — Jellyfin 12`**, attached assets: `titlovi-com_2.0.0.0.zip`, `SHA256SUMS.txt` (and optional MD5 sidecar). Release body from `RELEASE-NOTES-DRAFT.md`, finalized for publication by removing any provisional wording as appropriate.
5. Download the **published** ZIP independently; verify its exact 3 entries, metadata (GUID/version/ABI), SHA-256 and MD5; use checksum of those exact published ZIP bytes.
6. **Only then** append one version object at the beginning of `gh-pages/repository.json`. Preserve **all existing top-level fields byte-for-byte in meaning and all nine historical version objects**, without opportunistically modifying owner/category/description/overview. New version: `version: 2.0.0.0`, `targetAbi: 12.0.0.0`, `sourceUrl: https://github.com/Titlovi/titlovi-jellyfin/releases/download/v2.0.0/titlovi-com_2.0.0.0.zip`, `checksum: <MD5 of published zip>`, `timestamp: <UTC publish timestamp>`, `changelog: <short English sentence>`.
7. Independently fetch the public repository JSON and public ZIP; confirm 10 versions, nine original objects unchanged, same GUID, downloadable 2.0 ZIP whose MD5 matches the catalogue, and no broken legacy URLs.
8. Check visible Jellyfin documentation links and Matko's original video. Optionally confirm catalogue-based install/update on a separate Jellyfin 12 test server without disturbing the manually validated instance.

**Important:** The existing `scripts/migration/prepare-official-catalogue.py` makes review output that additionally rewrites top-level catalogue fields. **Do not publish its output as-is.** Make only the minimal new-version insertion when publishing.

## Status

- CI + manual functional acceptance: **PASS**.
- Automatic update from new public catalogue: **not yet tested**.
- Published 2.0 assets and final catalogue checksum: **not yet available**, do not fabricate.
- PR #7: **Draft, not merged**.
- Public release / catalogue: **not changed**.
