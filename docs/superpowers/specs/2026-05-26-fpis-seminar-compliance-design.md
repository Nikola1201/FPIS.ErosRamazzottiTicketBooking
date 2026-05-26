# FPIS.ErosRamazzottiTicketBooking — Seminar Compliance Design

**Date:** 2026-05-26
**Status:** Approved (pending user review of this document)
**Repo:** https://github.com/Nikola1201/FPIS.ErosRamazzottiTicketBooking
**Author:** Nikola1201 (`160769567+Nikola1201@users.noreply.github.com`)

## 1. Goal

Bring the existing `FPIS.ErosRamazzottiTicketBooking` repository into compliance with the seminar requirements **without changing the structure or behavior of any domain model**. Add an xUnit test project covering every domain class, XML documentation comments across `FPIS.Domain`, a documented .NET CLI workflow, and a Git history + branching topology that visibly demonstrates layered development and branch-merge discipline.

## 2. Requirements mapping

| Requirement | How this design satisfies it |
|---|---|
| 4–5 interconnected domain classes in C# | Repo already contains 12 interconnected classes under `Domain/Models/` (Concert, ConcertDate, Customer, Reservation, ReservationTicket, ReservationStatus, Discount, DiscountType, PromoCode, Zone, AccessToken, AppSettings). Untouched. |
| Git with student's email, visible history, multiple branches + merging, hosted | Repo on GitHub under `Nikola1201/...`, email `160769567+Nikola1201@users.noreply.github.com`. Phase 0 rewrites `main` into a layered story; Phase 1+ adds four feature branches merged into `main` with `--no-ff` so topology is visible. |
| .NET project, build/test/restore via .NET CLI | All operations performed via `dotnet restore`, `dotnet build`, `dotnet test`, `dotnet run`. A `global.json` pins the SDK band. No IDE-only or PowerShell-wrapper steps. |
| All domain classes tested with xUnit | New project `FPIS.Domain.Tests` (xUnit) added to the solution. Exhaustive property + boundary + invariant tests on every class in `Domain/Models/`. |
| Thorough XML documentation | `///` XML doc comments added to every public type and member in `FPIS.Domain` (Models, ViewModels, Mappings, Guards). `<GenerateDocumentationFile>true</GenerateDocumentationFile>` set on `FPIS.Domain.csproj`. |
| Works with JSON format (e.g., persistence) | Documented in `README.md`. Existing API already serializes/deserializes all DTOs and ViewModels via `System.Text.Json` (default in .NET 8). `appsettings.json` is bound via `IOptions<AppSettings>`. The `.http` file uses JSON bodies. No new JSON file persistence added — existing surface satisfies the requirement. |

## 3. Out of scope

- Any change to fields, methods, types, or behavior in `Domain/Models/`. Only `///` comment additions are permitted in those files.
- Tests on `FPIS.Infrastructure` or `FPIS.ErosRamazzottiTicketBooking.Api`. Requirement is scoped to domain classes.
- XML doc comments on `FPIS.Infrastructure` or `FPIS.ErosRamazzottiTicketBooking.Api`. (User decision to keep docs scope minimal.)
- New JSON file persistence code. Existing JSON surface is sufficient.
- The stale `master` branch. Backed up then deleted from remote.
- Refactoring, renaming, restructuring, or cleanup unrelated to the requirements above.

## 4. Phase 0 — History rewrite

The current `main` carries a mix of pushed commits and a large uncommitted WIP (modified domain models, infrastructure configurations, deleted migrations, new controllers). Rewriting history produces a coherent layered story that mirrors how the project would be built from scratch.

### 4.1 Safety net (before any destructive step)

```bash
git checkout main
git branch backup/pre-rewrite-main
git push origin backup/pre-rewrite-main

git checkout master
git branch backup/pre-rewrite-master
git push origin backup/pre-rewrite-master

git checkout main
```

Both branches are preserved on the remote. If the rewrite goes wrong, `git reset --hard origin/backup/pre-rewrite-main` restores `main` exactly.

### 4.2 Final tree capture

The rewrite represents the **final intended state** of the repository — the user's current working tree (HEAD + uncommitted WIP) is **the** desired final state, no manual edits needed. Steps:

1. On `main` with all current WIP present in the working tree (do not stash, do not discard).
2. `git checkout --orphan rewrite-main` — new branch with no parent; the working tree files (including WIP) are preserved automatically.
3. `git reset` — clear the index so nothing is staged; working tree files remain on disk.
4. Stage and commit in the order below. Each `git add` selects a specific subset of files; uncommitted files carry forward into later commits.

### 4.3 Target commit sequence (Serbian messages, conventional-commits scopes in English)

| # | Files staged | Commit message |
|---|---|---|
| 1 | `.gitattributes`, `.gitignore` | `chore: Dodao .gitattributes i .gitignore` |
| 2 | `FPIS.ErosRamazzottiTicketBooking.sln`, `Domain/FPIS.Domain.csproj`, `Infrastructure/FPIS.Infrastructure.csproj`, `FPIS.ErosRamazzottiTicketBooking.Api/FPIS.ErosRamazzottiTicketBooking.Api.csproj`, `FPIS.ErosRamazzottiTicketBooking.Api/Properties/` | `chore: Postavio solution i project fajlove` |
| 3 | `Domain/Models/Customer.cs`, `Concert.cs`, `ConcertDate.cs`, `Zone.cs` | `feat(domain): Dodao osnovne domenske modele` |
| 4 | `Domain/Models/Reservation.cs`, `ReservationTicket.cs`, `ReservationStatus.cs`, `Discount.cs`, `DiscountType.cs`, `PromoCode.cs`, `AccessToken.cs`, `AppSettings.cs` | `feat(domain): Dodao reservation agregat i prateće modele` |
| 5 | `Domain/ViewModels/*` | `feat(domain): Dodao view modele i DTO-ove` |
| 6 | `Domain/Mappings/*`, `Domain/Guards/*` | `feat(domain): Dodao mappings i guards` |
| 7 | `Infrastructure/ApplicationDbContext.cs`, `Infrastructure/Configurations/*` | `feat(infrastructure): Dodao DbContext i entity configurations` |
| 8 | `Infrastructure/Migrations/*` (final state only) | `feat(infrastructure): Dodao EF Core migracije` |
| 9 | `Infrastructure/Repositories/*` and any remaining files under `Infrastructure/` not covered by commits 7–8 | `feat(infrastructure): Dodao repozitorijume` |
| 10 | `FPIS.ErosRamazzottiTicketBooking.Api/Program.cs`, `Middleware/*`, `appsettings*.json`, `.http`, `Utility/*` | `feat(api): Dodao Program.cs, middleware i konfiguraciju` |
| 11 | `FPIS.ErosRamazzottiTicketBooking.Api/Services/*` | `feat(api): Dodao servise` |
| 12 | `FPIS.ErosRamazzottiTicketBooking.Api/Controllers/*` | `feat(api): Dodao kontrolere` |
| 13 | `docs/superpowers/specs/*` | `docs(spec): Dodao spec za seminar compliance` |

After commit 12 the working tree should be empty (`git status` clean). Any file not listed above either does not exist in the final tree or belongs to a single bucket above — verify with `git status` after each commit.

### 4.4 Replace `main` and force-push

```bash
git branch -M rewrite-main main          # rename orphan to main, overwriting old main locally
git push origin main --force-with-lease  # publish rewritten history
git push origin :master                  # delete stale master from remote
git branch -D master                     # delete stale master locally
```

`--force-with-lease` refuses to overwrite the remote if someone else has pushed in the meantime — safer than `--force`.

### 4.5 Acknowledged risks

1. All existing commit SHAs change. Any external reference (e.g., comments linking to specific commits) breaks. None known.
2. Other clones of this repo (other machines, collaborators) must re-clone or hard-reset. User confirmed this is a solo repo.
3. The two `backup/pre-rewrite-*` branches on remote are the recovery path.

## 5. Phase 1 — Test project (`feat/xunit-test-project`)

Branch from the rewritten `main`. Scope: scaffold the project and prove it runs.

### 5.1 Create + wire up

```bash
git checkout -b feat/xunit-test-project
dotnet new xunit -n FPIS.Domain.Tests -f net8.0 -o FPIS.Domain.Tests
dotnet sln add FPIS.Domain.Tests/FPIS.Domain.Tests.csproj
dotnet add FPIS.Domain.Tests/FPIS.Domain.Tests.csproj reference Domain/FPIS.Domain.csproj
```

Default packages from the xUnit template (`xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, `coverlet.collector`) are sufficient — no extras needed.

### 5.2 Smoke test

A single `SolutionWiringTests.cs` that asserts the test project can reference and instantiate one Domain type (e.g., `new Concert().Id == Guid.Empty`). Confirms project references work end-to-end before adding the bulk of tests.

### 5.3 Commits + merge

- One commit: `chore(tests): Dodao FPIS.Domain.Tests xUnit projekat`
- Verify: `dotnet test` from solution root passes with 1 test
- Merge: `git checkout main && git merge --no-ff feat/xunit-test-project -m "Merge feat/xunit-test-project"`
- Delete: `git branch -d feat/xunit-test-project && git push origin --delete feat/xunit-test-project`

## 6. Phase 2 — Domain test coverage (`test/domain-coverage`)

Branch from rewritten `main` after Phase 1 merges. Heavy coverage strategy per design decision.

### 6.1 Test layout

```
FPIS.Domain.Tests/
  Models/
    ConcertTests.cs
    ConcertDateTests.cs
    CustomerTests.cs
    DiscountTests.cs
    DiscountTypeTests.cs
    PromoCodeTests.cs
    ReservationTests.cs
    ReservationStatusTests.cs
    ReservationTicketTests.cs
    ZoneTests.cs
    AccessTokenTests.cs
    AppSettingsTests.cs
```

One test file per Domain model class. ViewModels, Mappings, and Guards are not required by the seminar wording ("all *domain classes*") but Mappings extension methods and `EmailMatchAttribute` will get tests too since they contain real behavior.

Additional files:
```
FPIS.Domain.Tests/
  Mappings/
    ConcertMappingsTests.cs
    ConcertDateMappingsTests.cs
    ReservationDetailsMappingsTests.cs
    ZoneMappingsTests.cs
  Guards/
    EmailMatchAttributeTests.cs
```

### 6.2 Coverage strategy per class type

**Pure data POCOs** (`Concert`, `ConcertDate`, `Customer`, `Zone`, `AccessToken`, `AppSettings`, `ReservationTicket`):
- Default-value test per property (verify initialization)
- Round-trip set/get per property
- Collection-init test for navigation collections (e.g., `Concert.Dates` non-null, empty)
- Reference-property `default!` invariant (assigning then reading back returns same instance)
- Boundary tests on numeric/string fields (zero, MaxValue, empty, Unicode, very long strings)

**Aggregate root + relationships** (`Reservation`, `PromoCode`, `Discount`):
- All POCO tests above, plus:
- Bidirectional wiring tests (assign related entity, assert nav + FK property values stay consistent)
- Optional-relationship tests (nullable FKs default to null; assigning sets both id + nav)

**Enums** (`DiscountType`, `ReservationStatus`):
- Stability test: assert the exact set of defined names exists (`Enum.GetNames<T>()` matches expected array)
- Numeric backing-value stability test: each defined member has expected `int` value (catches accidental reordering)
- `IsDefined` test: a known value returns true; a sentinel out-of-range value returns false

**Mappings** (`ConcertMappings`, `ConcertDateMappings`, `ReservationDetailsMappings`, `ZoneMappings`):
- For each public extension method: input → expected output mapping test
- Null-input handling test where the method is supposed to accept null
- Collection-input test where applicable

**Guards** (`EmailMatchAttribute`):
- `IsValid` returns true when both email fields match
- `IsValid` returns false when they differ
- Null/empty handling per attribute's contract

### 6.3 Target volume

≥80 tests total. The number is a side effect of doing the above per class, not a target to hit by padding.

### 6.4 Commits + merge

Commits grouped by file family — roughly one commit per 3–5 test files to keep diffs reviewable:
- `test(domain): Dodao testove za osnovne modele (Concert, ConcertDate, Customer, Zone)`
- `test(domain): Dodao testove za reservation agregat (Reservation, ReservationTicket, Discount, PromoCode)`
- `test(domain): Dodao testove za enum-ove (DiscountType, ReservationStatus)`
- `test(domain): Dodao testove za AccessToken i AppSettings`
- `test(domain): Dodao testove za mappings i guards`

Verify after each commit: `dotnet test` passes, total count grows. Final merge `--no-ff` into `main` and branch deleted.

## 7. Phase 3 — XML documentation (`docs/xml-comments-domain`)

Branch from `main` after Phase 2 merges. Scope: `FPIS.Domain` only.

### 7.1 Project setup

Edit `Domain/FPIS.Domain.csproj`:

```xml
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

CS1591 (missing-doc warning) stays enabled — it produces a visible warning per public member without a `///` comment, giving us a checklist.

### 7.2 Coverage

Every public type and public member in:
- `Domain/Models/*.cs` (12 files) — `<summary>` on the class and each property
- `Domain/ViewModels/*.cs` (11 files) — same
- `Domain/Mappings/*.cs` (4 files) — `<summary>` on the class and each extension method; `<param>` on each parameter; `<returns>` on the return value
- `Domain/Guards/EmailMatchAttribute.cs` — `<summary>` on the class, constructor, properties, and `IsValid` override; `<param>` and `<returns>` on `IsValid`; `<remarks>` explaining intent

No code-behavior changes. Build target after the work: zero CS1591 warnings in `FPIS.Domain`, all other projects unchanged.

### 7.3 Commits + merge

Grouped by folder to keep diffs reviewable:
- `docs(domain): Dodao XML komentare na Models`
- `docs(domain): Dodao XML komentare na ViewModels`
- `docs(domain): Dodao XML komentare na Mappings i Guards`
- `chore(domain): Uključio GenerateDocumentationFile u FPIS.Domain.csproj`

Verify `dotnet build` shows zero CS1591 warnings in `FPIS.Domain`. Merge `--no-ff`, delete branch.

## 8. Phase 4 — README + CLI workflow (`docs/seminar-readme`)

Branch from `main` after Phase 3 merges.

### 8.1 Files added

- `README.md` at repo root
- `global.json` at repo root pinning SDK band:
  ```json
  {
    "sdk": {
      "version": "8.0.100",
      "rollForward": "latestFeature"
    }
  }
  ```

### 8.2 README.md sections

1. **Pregled projekta** — short description of the FPIS Eros Ramazzotti ticket-booking API
2. **Arhitektura** — three-layer overview: `Domain`, `Infrastructure`, `FPIS.ErosRamazzottiTicketBooking.Api`; one paragraph per layer
3. **Zahtevi** — .NET 8 SDK
4. **Pokretanje preko .NET CLI** — canonical commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet test
   dotnet run --project FPIS.ErosRamazzottiTicketBooking.Api
   ```
5. **Korišćenje JSON formata** — explicit enumeration of JSON usage in the project:
   - ASP.NET Core controllers serialize/deserialize all DTOs and ViewModels via `System.Text.Json` (default JSON serializer in .NET 8). Example: `HomeController.GetHomePage()` returns `HomePageViewModel` which is serialized to JSON automatically.
   - `appsettings.json` + `appsettings.Development.json` bound to `AppSettings` via `IOptions<AppSettings>`. Example reference: `Program.cs`, `Domain/Models/AppSettings.cs`.
   - `.http` test file uses JSON request bodies for manual API testing.
6. **Git workflow** — GitHub Flow, conventional commits, `--no-ff` merges, branch list with one-line purpose per branch
7. **Testovi** — how to run tests, where they live, coverage philosophy
8. **Licenca** — placeholder or none

### 8.3 Commits + merge

- `docs: Dodao README sa pregledom, CLI workflow-om i JSON sekcijom`
- `chore: Dodao global.json sa .NET 8 SDK pinom`

Merge `--no-ff`, delete branch.

## 9. Final tag

After all four feature branches are merged:

```bash
git checkout main
git tag -a v1.0-seminar -m "Seminar predaja: kompletni testovi, XML dokumentacija, CLI workflow"
git push origin v1.0-seminar
```

## 10. Verification checklist

Before declaring the work complete:

- [ ] `git log --graph --oneline --all` shows: rewritten layered history → 4 `--no-ff` merge commits visible on `main` → `v1.0-seminar` tag on tip of `main`
- [ ] `git status` clean on `main`
- [ ] `git remote show origin` confirms `main`, `backup/pre-rewrite-main`, `backup/pre-rewrite-master`, `v1.0-seminar` exist; `master` deleted
- [ ] `dotnet restore` succeeds from solution root
- [ ] `dotnet build` succeeds with zero CS1591 warnings in `FPIS.Domain`
- [ ] `dotnet test` runs ≥80 tests, all pass
- [ ] `dotnet run --project FPIS.ErosRamazzottiTicketBooking.Api` starts the API without errors (smoke check only)
- [ ] `README.md` present and accurate; `global.json` present
- [ ] `git config user.email` matches `160769567+Nikola1201@users.noreply.github.com`
- [ ] Final `main` pushed to origin
- [ ] Both `backup/pre-rewrite-*` branches still on remote (safety net retained for the grading period; can be deleted afterward)

## 11. Open questions resolved during brainstorming

- **XML docs on existing models** — allowed because `///` additions don't change behavior or structure.
- **WIP commits in tree** — folded into Phase 0 rewrite.
- **JSON functionality** — existing ASP.NET Core JSON serialization + `appsettings.json` binding satisfies the requirement; documented in README, no new persistence code.
- **Branching strategy** — GitHub Flow + Conventional Commits + `--no-ff` merges.
- **Test scope** — exhaustive property + boundary + invariant tests per class.
- **Test project structure** — single `FPIS.Domain.Tests` project.
- **XML doc scope** — `FPIS.Domain` only.
- **History rewrite** — full destructive rewrite with backup branches on remote.
- **Commit language** — Serbian messages, English conventional-commits scopes.
