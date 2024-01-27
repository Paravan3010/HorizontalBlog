# Konfigurace aplikace a první spuštění
Vytvořte kopii souboru appsettings.Example.json, přejmenujte jej na appsettings.Development (případně appsettings.Production) a nakonfigurujte podle popisu níže.

## Konfigurace v appsettings.json
Vyžadovaná konfigurace:
1) Connection string do databáze v uzlu *ConnectionStrings:MainConnection*. Např.: *Server=**SERVER**;Database=**DATABASE**;Trusted_Connection=True;MultipleActiveResultSets=true;TRUSTSERVERCERTIFICATE=yes;*
2) Přihlašovací jméno a heslo do admin části webu v uzlech *AdminUser:Login* a *AdminUser:Password*. Heslo musí splňovat defaultní bezpečnostní standardy: musí obsahovat velké písmeno, malé písmeno, číslici a nealfanumerický znak. Zároveň nesmí být kratší, než 6 znaků. Pokud toto nebude splněno, nebude účet při prvním spuštění založen a nebude možné se přihlásit do Admin sekce. Pokus o založení účtu proběhne při každém dalším spuštění. Stačí tedy překonfigurovat heslo a spustit aplikaci znovu.

Volitelná konfigurace:
1) Naplnění databáze příkladovými testovacími daty v uzlu *FillWithTestData*. Tato příkladová data používají články a obrázky uložené v tomto repozitáři ve složkách wwwroot/articles a wwwroot/img. Testovací záznamy se do DB vloží pouze v případě, že v ní již nejsou uloženy jiné záznamy. Toto nastavení je relevantní pouze pro vývojové prostředí, na produkčním prostředí není zohledněno a testovací data tam nejsou naplněna nikdy.

## Mapování vlastních URL
Mapování vlastních URL probíhá v Administraci aplikace v sekci _Mapování URL_. 
**Domovskou stránku** je pro korektní fungování doporučeno **mapovat na prázdný řetězec**. Tj. zavést mapování "_/Home/Main_" => "".

## Spuštění aplikace pro vývoj
Po vytvoření a nakonfigurování spojení do databáze (viz výše) jsou při prvním spuštění aplikovány migrace, které v ní vytvoří příslušné struktury. Pokud je zapnuto plnění testovacími daty (viz výše) a jedná se o vývojové prostředí, jsou tyto struktury navíc naplněny příkladovými daty. Při spuštění z Visual Studia tak nejsou potřeba žádné další kroky.
