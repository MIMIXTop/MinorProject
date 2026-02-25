# MinorProject

Приложение на базе .NET Avalonia с MVVM архитектурой.

## Технологии

- .NET 10.0
- Avalonia 11.3.11
- CommunityToolkit.Mvvm 8.2.1

## Структура проекта

```
MinorProject/
├── App.axaml              # Главный XAML файл
├── App.axaml.cs           # Главный код приложения
├── Program.cs             # Точка входа
├── ViewLocator.cs         # Локатор представлений
├── MinorProject.csproj    # Файл проекта
├── Views/                 # Представления (GUI)
│   ├── MainWindow.axaml
│   └── MainWindow.axaml.cs
├── ViewModels/            # ViewModels (MVVM)
│   ├── MainWindowViewModel.cs
│   └── ViewModelBase.cs
└── Models/                # Модели данных
```

## Установка и запуск

1. Установите .NET 10 SDK
2. Откройте терминал в папке проекта
3. Запустите приложение:
   ```bash
   dotnet run
   ```

## Как создать Pull Request

Если вы хотите внести вклад в проект, следуйте этим шагам:

### 1. Fork репозитория

1. Перейдите на [GitHub репозиторий](https://github.com/MIMIXTop/MinorProject)
2. Нажмите кнопку **Fork** в правом верхнем углу страницы
3. Скопируйте URL вашего форка

### 2. Клонирование репозитория

```bash
git clone https://github.com/ВАШ_НИК/minorproject.git
cd minorproject
```

### 3. Создание ветки для изменений

```bash
# Проверьте текущую ветку
git branch

# Создайте новую ветку с описательным именем
git checkout -b feature/ВАША_ФУНКЦИЯ
# Например: git checkout -b feature/new-authentication
```

### 4. Внесение изменений

1. Внесите изменения в код
2. Сохраните файлы

### 5. Коммиты

```bash
# Посмотрите изменения
git status

# Добавьте измененные файлы
git add .

# Создайте коммит с описательным сообщением
git commit -m "Описание ваших изменений"
# Например: git commit -m "Add user authentication feature"
```

### 6. Пуш в удаленный репозиторий

```bash
# Отправьте ветку в ваш форк
git push origin feature/ВАША_ФУНКЦИЯ
# Например: git push origin feature/new-authentication
```

### 7. Создание Pull Request

1. Перейдите на [ваш форк репозитория](https://github.com/ВАШ_НИК/minorproject)
2. Вы увидите кнопку **Compare & pull request** - нажмите её
3. Заполните информацию о PR:
   - **Title**: Краткое описание изменений
   - **Description**: Подробное описание изменений, чего добавили и почему
4. Нажмите кнопку **Create pull request**

## Стандарты оформления коммитов

Используйте следующие форматы для коммитов:

```
feat: добавление новой функции
fix: исправление ошибки
docs: обновление документации
style: изменения в коде, не влияющие на логику
refactor: рефакторинг кода
test: добавление тестов
chore: технические изменения (build, dependencies, etc.)
```

Примеры:

```bash
git commit -m "feat: add user authentication system"
git commit -m "fix: resolve login bug on empty fields"
git commit -m "docs: update README with PR instructions"
git commit -m "refactor: improve ViewModel structure"
```

## Лицензия

Этот проект находится в разработке. Пожалуйста, ознакомьтесь с файлом LICENSE, если он существует.

## Контакт

Для вопросов и предложений свяжитесь с автором проекта.