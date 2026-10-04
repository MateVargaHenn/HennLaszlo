# Visszaállítás ütközésvédelme

Ág: `fix/content-revision-restore-conflicts`.

## Alkalmazás

Csomagold ki a ZIP-et a repository gyökerébe (a backend és frontend mellé).
Tiszta munkakönyvtárból, a fenti ágon futtasd:

```powershell
git apply --check ./restore-conflicts.patch
git apply ./restore-conflicts.patch
```

Ha az ellenőrzés hibát ad, ne alkalmazd a patch-et; küldd el a hibát.
A módosítás 14 fájlt érint; nem szükséges adatbázis-migráció.

## Ellenőrzés

A frontend/henn-laszlo mappában:

```powershell
npx ng build content-data-access
npx ng build admin-web
npx ng test admin-web --watch=false
```

A repository gyökerében:

```powershell
dotnet build backend/HennLaszlo.slnx
dotnet test backend/HennLaszlo.slnx --no-build
git diff --check
```

Indítsd újra a backendet. Manuálisan mind írással, mind tartalmi oldallal:

1. Az első fülön nyisd meg egy korábbi verzió visszaállításának megerősítését.
2. A második fülön módosítsd és mentsd ugyanazt a tartalmat.
3. Az első fülön erősítsd meg a visszaállítást: 409 és külön ütközési üzenet az elvárt eredmény. Nem jelenik meg sikerjelzés, a második fül mentése megmarad, és nem keletkezik új pillanatkép ebből az elutasított kérésből.
4. Töltsd újra az első fület, és ismételd meg a visszaállítást másik mentés nélkül: sikeres visszaállítás az elvárt eredmény. A visszaállítás előtti állapot megmarad az előzmények között.

## Tartalom

A szerkesztő a formba betöltött verziót adja át a közös komponensnek. A komponens és a store továbbítja az API-nak. A backend a verziót mindkét tartalomtípusnál a pillanatkép készítése és a módosítás előtt ellenőrzi. A már beállított EF concurrency token az ellenőrzés és mentés között bekövetkező másik módosítást is védi.

Ütközéskor a kiválasztott előnézet megmarad, nem fut le a sikeres visszaállítás eseménye, és az ismételt megerősítés le van tiltva az ablak bezárásáig. Nincs automatikus verziófrissítés vagy felülírás.

Új tesztek: verzióküldés a valódi store-on keresztül mindkét tartalomtípusnál; sikertelen és sikeres visszaállítás; hiányzó verzió; a megerősítés megnyitása után keletkezett nem mentett módosítások védelme.

A csomag TypeScript-szintaxisát, diffjét és patch-alkalmazását ellenőriztem. A teljes Angular és .NET build és teszt ebben a környezetben nem futott; ezeket a fenti parancsokkal helyben ellenőrizd.

Sikeres ellenőrzés után következik a 0.5.5 -> 0.5.6 verzióemelés, commit és PR. A csomag a verziószámot nem módosítja.
