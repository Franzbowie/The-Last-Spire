# Death Shop UI Setup Instructions

## Структура UI для магазина смерти

### Основной Canvas
- **DeathShopPanel** (GameObject)
  - **CoinsDisplay** (Text/TMP_Text) - отображение монет
  - **TabButtons** (GameObject)
    - **UpgradesButton** (Button) - кнопка вкладки "Улучшения"
    - **PassivesButton** (Button) - кнопка вкладки "Пассивное"
    - **StatsButton** (Button) - кнопка вкладки "Статистика"
  - **TabPanels** (GameObject)
    - **UpgradesTab** (GameObject) - панель улучшений
      - Кнопки покупки улучшений
      - Тексты с ценами
    - **PassivesTab** (GameObject) - панель пассивов
      - Кнопки покупки пассивов
      - Тексты с ценами
    - **StatsTab** (GameObject) - панель статистики
      - **StatsView** (компонент) - отображение статистики
  - **RestartButton** (Button) - кнопка перезапуска

### Настройка в Inspector

1. **DeathShopController** компонент:
   - **Upgrades Tab**: назначить UpgradesTab GameObject
   - **Passives Tab**: назначить PassivesTab GameObject
   - **Stats Tab**: назначить StatsTab GameObject
   - **Upgrades Button**: назначить UpgradesButton
   - **Passives Button**: назначить PassivesButton
   - **Stats Button**: назначить StatsButton
   - **Coins Text**: назначить CoinsDisplay Text
   - Назначить все тексты с ценами

2. **Кнопки вкладок**:
   - UpgradesButton → OnClick → ShowUpgrades()
   - PassivesButton → OnClick → ShowPassives()
   - StatsButton → OnClick → ShowStats()

3. **StatsView** компонент на StatsTab:
   - Назначить все текстовые поля для статистики

### Поведение
- По умолчанию открыта вкладка "Улучшения"
- Только одна вкладка видна одновременно
- Кнопки вкладок меняют цвет (белый = активна, серый = неактивна)
- При переключении вкладок обновляются данные
