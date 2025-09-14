# Weapon Selection UI Setup Instructions

## Структура UI для системы выбора оружия

### Обновленная структура UpgradesTab
- **UpgradesTab** (GameObject)
  - **WeaponSelectionPanel** (GameObject)
    - **WeaponScrollRect** (ScrollRect) - прокручиваемый список кнопок оружия
      - **Viewport** (GameObject)
        - **Content** (GameObject) - контейнер для кнопок оружия
          - **WeaponButtonPrefab** (Button) - префаб кнопки оружия
  - **UpgradePanelsContainer** (GameObject)
    - **SkyStrikeUpgrades** (GameObject) - панель улучшений небесного снаряда
    - **SeekerUpgrades** (GameObject) - панель улучшений искателя
    - **MultishotUpgrades** (GameObject) - панель улучшений мультивыстрела
    - **SwordsUpgrades** (GameObject) - панель улучшений мечей
    - **RunesUpgrades** (GameObject) - панель улучшений рун
    - **AoEEMIUpgrades** (GameObject) - панель улучшений ЭМИ
    - **BouncerUpgrades** (GameObject) - панель улучшений вышибала
    - **MortarUpgrades** (GameObject) - панель улучшений мортиры
    - **BeamUpgrades** (GameObject) - панель улучшений луча
    - **ShieldUpgrades** (GameObject) - панель улучшений щита

### Настройка в Inspector

1. **DeathShopController** компонент:
   - **Weapon Scroll Rect**: назначить ScrollRect компонент
   - **Weapon Button Container**: назначить Content GameObject из ScrollRect
   - **Weapon Button Prefab**: создать префаб кнопки оружия
   - **Upgrade Panels**: назначить все панели улучшений

2. **WeaponButtonPrefab** структура:
   - **Button** компонент
   - **Text** компонент для названия оружия
   - Настроить размеры и стиль кнопки

3. **ScrollRect** настройки:
   - **Horizontal**: false (вертикальная прокрутка)
   - **Vertical**: true
   - **Movement Type**: Elastic
   - **Scroll Sensitivity**: 20

### Поведение системы

1. **Инициализация**:
   - При открытии магазина создаются кнопки для всех видов оружия
   - По умолчанию выбран "Небесный снаряд"

2. **Выбор оружия**:
   - Клик по кнопке оружия переключает на его улучшения
   - Скрываются все панели улучшений кроме выбранной
   - Очищаются и обновляются цены для выбранного оружия

3. **Прокрутка**:
   - Кнопки оружия прокручиваются вертикально
   - Показывается только часть кнопок одновременно

### Список оружия (в порядке enum)
1. Небесный снаряд (SkyStrike)
2. Искатель (Seeker)
3. Мультивыстрел (Multishot)
4. Мечи (Swords)
5. Руны (Runes)
6. ЭМИ (AoEEMI)
7. Вышибала (Bouncer)
8. Мортира (Mortar)
9. Луч (Beam)
10. Щит (Shield)

### Расширение системы

Для добавления улучшений для нового оружия:
1. Создать панель улучшений в UI
2. Добавить поля цен в DeathShopController
3. Реализовать методы покупки
4. Добавить case в RefreshWeaponPrices()
