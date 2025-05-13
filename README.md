# 🛸 Spline Követős Játék (Unity – 2D)

Ez a projekt egy egyszerű 2D játék Unity-ben, ahol egy karaktert (pl. űrhajót) kell az egérrel irányítani az X tengely mentén, miközben a pálya felfelé mozog. A cél, hogy a karakter egy **B-spline** alapján generált útvonalon maradjon.

## 🎮 Játékmenet

- A játékos karakter az egérrel mozgatható vízszintesen.
- A pálya végtelenített módon halad felfelé.
- A spline random generált kontrollpontokból épül fel.
- A játék célja, hogy minél pontosabban kövessük az útvonalat.

## 🧱 Megvalósítás

- A spline-t B-spline algoritmussal generálom.
- A játéktér két oldalát külön színnel jelenítem meg, a spline mentén generált poligonokkal.
- A pálya mozog, nem a karakter – így a koordináták stabilak maradnak.


---

**Készítette:** Biszterszky Mátyás – L27NCJ
