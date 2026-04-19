# beheersysteem-uitvaartcentrum

### Threat #25/#32 – Cross-Site Scripting (XSS)

### Gewijzigde bestanden
- `beheersysteem_uitvaartcentrum.backend.api/Filters/SanitizeInputFilter.cs` — nieuw bestand
- `beheersysteem_uitvaartcentrum.backend.api/Program.cs` — filter globaal geregistreerd

### Wat is gewijzigd
Er is een globale Action Filter toegevoegd die automatisch alle inkomende 
gebruikersinput saniteerd. De filter encodeert gevaarlijke tekens in alle 
string-properties van DTOs via `HtmlEncoder.Default.Encode()`, waardoor 
tekens zoals `<` en `>` worden omgezet naar HTML-entiteiten (`&lt;`, `&gt;`).

### Waarom
Zonder sanitatie kan een aanvaller kwaadaardige scripts zoals `<script>` 
injecteren via API-requests. Door input centraal te saniteren in een globale 
filter hoeft dit niet per controller of methode handmatig gedaan te worden, 
en wordt de volledige API beschermd.
