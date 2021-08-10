# Situación  GEODOCAT en CdD

# Exportación al CdD del CartoSEE

## Situación

Hemos solicitado al **CdD** la infomación de los ficheros que tenemos disponibles a descarga para incorporarla a **BADASID**. Nos han enviado unos SHP con las geometrías de los documento a descarga y dos excel con los documentos sin geometría que no están a descarga.

- Fichero Contornos Planimetrías, Altimetrías y Conjuntas: 33866 elementos.
- Fichero Contornos Planos de Población: 9720 elementos.
- Fichero Contornos de Planos de edificios: 433 elementos.
- Excel de Planimetrías, Altimetrías y Conjuntas sin geometría: 1574
- Excel de Planos de edificios sin geometría: 2

> 👀 Cuando un documento no tiene geometría asociada, no está disponible para su descarga 🤷‍♂️.

Se puede acceder de manera individual en el CdD a un fichero mediante una URL, pero sólo está disponible para el producto *Planimetrías, Altimetrías y Conjuntas*.

## 🕵️‍♂️ Análisis

### 🔸 Documentos de GEODOCAT que faltan en el CdD

Dados los productos del CdD, nos falta subir la siguiente información al CdD.

A partir de esta consulta, obtenemos que:

```sql
select tbtipodocumento.tipodoc,*from archivoinner join tbtipodocumento on archivo.tipodoc_id=tbtipodocumento.idtipodocwhere cdd_producto='No disponible en el CdD' and tbtipodocumento.tipodoc in('Planimetría','Altimetría','Conjunta','Plano de población','Edificación')
```

[Documentos de GEODOCAT sin pasar al CdD](https://www.notion.so/6f97de48ffd24babad7ebb3071b8c133)

### 🔸 Documentos en CdD mal etiquetados en el producto

Combinando diversas versiones de esta consulta, obtenemos que:

```sql
select tbtipodocumento.tipodoc,*from archivoinner join tbtipodocumento on archivo.tipodoc_id=tbtipodocumento.idtipodocwhere cdd_producto='Planos de población' and tbtipodocumento.tipodoc in('Planimetría','Altimetría','Conjunta','Edificación')
```

- Tenemos 5 *Planos de población* en el CdD que son realmente **Planos de edificación**.
- Tenemos 3 *Planimetrías, Altimetrías y Conjuntas* en el CdD que son realmente **Croquis de triangulación**.

### 🔸 Documentos en CdD mal etiquetados en el título

Combinando diversas versiones de esta consulta, obtenemos que:

```sql
select tbtipodocumento.tipodoc,*from archivoinner join tbtipodocumento on archivo.tipodoc_id=tbtipodocumento.idtipodocwhere cdd_titulo like 'PLANI%' and tbtipodocumento.tipodoc <>'Planimetría'
```

- Tenemos 18 documentos en el CdD como planimetrías que no lo son.
- Tenemos 6 documentos en el CdD como altimetrías que no lo son.
- Tenemos 13 documentos en el CdD como conjuntas que no lo son.

### 🔸 Documentos en CdD sin geometría

Como os comenté antes, si al CdD no se le pasa una geometría asociada al documento, no se pone a descarga. En esta situación tenemos **1574** documentos, que podemos obtener así

```sql
select tbtipodocumento.tipodoc,archivo.*from bdsidschema.archivoinner join tbtipodocumento on archivo.tipodoc_id=tbtipodocumento.idtipodocwhere cdd_producto = 'Planimetrías, Altimetrías y Conjuntas' and cdd_geometria=0
-- Resultado 1574
```

De estos documentos, a día de hoy tenemos **1136** documentos con contorno asociado,

```sql
select tbtipodocumento.tipodoc,archivo.*from archivoinner join tbtipodocumento on archivo.tipodoc_id=tbtipodocumento.idtipodocwhere cdd_producto = 'Planimetrías, Altimetrías y Conjuntas' and cdd_geometria=0 and archivo.idarchivo in (select archivo_id from bdsidschema.contornos)
-- Resultado 1136
```

por lo que faltarían **438** sin geometría. Podemos obtener los que nos faltan así.

## Conclusiones

Actualmente tenemos tres productos en el CdD relacionados con el proyecto **GEODOCAT** y consultable a través de **CartoSEE**.

- Planimetrías, Altimetrías y Conjuntas.
- Planos de población.
- Planos de edificios.

con el siguiente desglose de documentos

[Documentos de GEODOCAT disponibles en el CdD](https://www.notion.so/40334a96cf5a4b3899a1d71ff1a6def9)

Todos los títulos que aparecen al usuario están sacados a partir de los nombres de fichero y no aportan la información necesaria para hacer búsquedas. Sería necesario incluir en los títulos de los ficheros información sobre anejos, localidades, descripciones de edificios representados, que permitan al usuario realizar búsquedas. Además los documentos deberían tener nombres más sencillos, indicando tan solo el tipo de documento y su sellado.

Los nombres de municipios deberían estar escritos correctamente, no como ahora, con abreviaturas y los acentos suprimidos. Todo esto no afectaría a la carga que se está haciendo en ABSYS, ya que las URL de consulta sólo emplean el idProductor, que se corresponde con el sellado.

Por otra parte, habría que solicitar que el resto de documentación también se sirviera mediante acceso directo usando su idProductor.

Los pasos que sugiero que deberíamos dar son:

- Hablar con el CdD y comentarles que les vamos a generar de nuevo toda la documentación, con los títulos corregidos y las asociaciones de contornos corregidas. Tiene sentido plantearlo porque desde 2017 se han corregido una gran cantidad de documentos, y no tenemos hasta ahora un control de versiones. Además, los datos que en aquella ocasión se suministraron eran de acuerdo a la limitaciones del CdD anterior, y la información asociada a los documentos no podía ser más completa.
- Implementar el control de versiones.
- Generar todos los contornos que faltan.
- Generar los ficheros desde cero, como hicimos en 2020 con las Actas y Cuadernos.