---
title: Guia contenido Starligth
description: Cómo crear contenido enriquecido
---

https://starlight.astro.build/es/guides/authoring-content/

```js {4} "Términos individuales" /También.*compatibles/
// mi-archivo-de-prueba.js
// Términos individuales también pueden ser resaltados
function demo() {
  return 'También las expresiones regulares son compatibles';
}
```

```js "return true;" ins="insertados" del="eliminados"
function demo() {
  console.log('Estos son tipos de marcadores insertados y eliminados');
  // La declaración de retorno utiliza el tipo de marcador predeterminado
  return true;
}
```

```bash title="Instalando dependencias…"
npm install
```

```bash frame="none"
echo "Esto no se renderiza como una terminal a pesar de usar el lenguaje bash"
```

:::note
Starlight es un conjunto de herramientas para sitios de documentación construido con [Astro](https://astro.build/). Puedes comenzar con este comando:

```sh
npm run create astro@latest --template starlight
```

:::

:::tip[¿Sabías esto?]
Astro te ayuda a construir sitios web más rápidos con la[“Arquitectura de Islas”](https://docs.astro.build/es/concepts/islands/).
:::

:::caution
Si no estás seguro de si deseas un sitio de documentación increíble, piénsalo dos veces antes de usar [Starlight](/es/).
:::

:::danger
Tus usuarios pueden ser más productivos y encontrar más fácil de usar tu producto gracias a las útiles características de Starlight.

- Navegación clara
- Tema de color configurable por el usuario
- [Soporte de i18n](/es/guides/i18n/)

:::

## Further reading

- Read [about how-to guides](https://diataxis.fr/how-to-guides/) in the Diátaxis framework
