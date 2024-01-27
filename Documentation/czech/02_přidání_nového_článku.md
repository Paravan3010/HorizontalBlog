# Založení nového článku
V admin sekci přidám nový článek a vyplním udáje.

## Text článku
Text článku je uložen jako HTML soubor, který je dynamicky dočítán při zobrazení detailu článku.

## Obrázky článku
Veškeré obrázky v článku musí být umístěny v lokaci odpovídající odkazům v HTML souboru. Momentálně se předpokládají pouze horizontálně orientované obrázky.
Veškeré obrázky musí být vyexportovány v horizontálním rozlišení **1565 px** a ve složce musí být umístěny jak ve formátu *WebP*, tak ve spádovém formátu *JPG*. Toto musí být reflektováno ve všech obrázcích vložených v HTML souboru článku (viz vzorový post).
V každém **img** tagu uvnitř článku uvádějte *width* i *height*, aby se předešlo posouvání layoutu při jejich postupném dočítání u pomalého internetového spojení. Zároveň je doporučeno všem obrázkům článku přidat atribut *loading="lazy"*, který umožní jejich líné načítání.
Náhledový obrázek pro *Big Article Preview* (použit např. na hlavní stránce a při filtrování dle kategorie nebo tagu) bude zobrazen v horizontální orientaci ve formátu **2:1**. U obrázků uvnitř článku je poměr stran zachován.

## Publikace článku
Články, které nejsou označeny jako publikované, jsou viditelné pouze pod admin přihlášením. Bez něj je při přístupu na URL článku vrácen error *404 Not Found*.

## Levé navigační menu
Pro korektní zahrnutí článku do levého navigačního menu je potřeba nastavit *krátký titulek*, pod kterým se v levém manu zobrazí, a také je potřeba korektně nastavit nadřazenou kategorii.
Levé navigační menu se generuje při startu aplikace. Aby se v něm projevila změna (přidání nového článku), je potřeba vynutit aktualizaci pomocí tlačíka *Aktualizovat levé navigační menu* v panelu *Obecné nastavení* v admin sekci.