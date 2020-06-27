export function generateTitle(title) {
  const hasKey = this.$te('Blocks.' + title)

  if (hasKey) {
    // $t :this method from vue-i18n, inject in @/lang/index.js
    const translatedTitle = this.$t('Blocks.' + title)
    return translatedTitle
  }
  return title
}
