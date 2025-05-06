// @ts-check
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";
import starlightDocSearch from "@astrojs/starlight-docsearch";
import { loadEnv } from "vite";

const { ALGOLIA_API_KEY, ALGOLIA_APP_ID, ALGOLIA_INDEX_NAME } = loadEnv(
  process.env.NODE_ENV ?? "",
  process.cwd(),
  "",
);

// https://astro.build/config
export default defineConfig({
  site: "https://gongarce.github.io",
  base: "/AvaloniaTutorial",
  integrations: [
    starlight({
      title: "AvaloniaUI Tutorial",
      social: {
        github: "https://github.com/gongarce/avalonia-tutorial",
      },
      sidebar: [
        {
          label: "Introducción",
          translations: {
            en: "Introduction",
          },
          autogenerate: { directory: "0-intro" },
        },
        {
          label: "Crear la ventana principal",
          translations: {
            en: "Create main window",
          },
          autogenerate: { directory: "1-main-window" },
        },
        {
          label: "Mostrar albumes",
          translations: {
            en: "Show albums",
          },
          autogenerate: { directory: "2-album" },
        },
        {
          label: "Venta de búsqueda",
          translations: {
            en: "Search window",
          },
          autogenerate: { directory: "3-dialog" },
        },
        {
          label: "Comprar álbum",
          translations: {
            en: "Buy album",
          },
          autogenerate: { directory: "4-buy" },
        },
        {
          label: "Últimos retoques",
          translations: {
            en: "Last details",
          },
          autogenerate: { directory: "5-last" },
        },
        {
          label: "Un paso más allá",
          translations: {
            en: "A step further",
          },
          autogenerate: { directory: "6-more" },
        },
      ],
      defaultLocale: "root",
      locales: {
        root: {
          label: "Español",
          lang: "es",
        },
        en: {
          label: "English",
          lang: "en",
        },
      },
      plugins: [
        // starlightDocSearch({
        //   appId: ALGOLIA_APP_ID,
        //   apiKey: ALGOLIA_API_KEY,
        //   indexName: ALGOLIA_INDEX_NAME,
        // }),
      ],
    }),
  ],
});
