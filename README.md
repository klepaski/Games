# Games
В проекте содержится 2 игры - Крокодил и Виселица.
Стартовая страница приложения:
![Home](https://github.com/klepaski/Games/assets/43060010/a6de725b-af69-4dcd-8ac3-b8274bad2b9c)

<h3>КРОКОДИЛ</h3>
При входе в игру предлагается выбрать роль - Крокодил или Игрок.
При выборе роли "Крокодил" создается новая игровая сессия (группа SignalR).
В каждой группе хранится два username - Крокодила и Игрока.

Когда другой игрок зайдет в игру с ролью Игрока, ему будет предложено ввести имя пользователя Крокодила, с которым он хочет играть.
Если такого имени среди крокодилов нет или данный пользователь уже играет с кем-то другим, будет выведено соответствующее сообщение.
Если во время игры один из игроков покинет страницу, игра окончится для обоих.
Аналогично в игре Виселица.

Начальная страница игры Крокодил:
![Croc1](https://github.com/klepaski/Games/assets/43060010/69e83f0d-cee0-4dc6-8a9b-f21b72e765af)

Предлагается выбрать 1+ категорию из предложенных:
* Профессии
* Природа
* Другое
* Известные люди
* Персонажи

После выбора категорий выдаются слова, которые можно пропустить или выбрать. При пропуске всех предложенных слов, 
они предлагаются заново сначала:

![Croc2](https://github.com/klepaski/Games/assets/43060010/d576e0ea-b929-48d9-aac9-2d4e8c7e7d3e)

Выберем какое-то слово. Далее мы попадем в игровую комнату, где крокодил должен нарисовать выбранное слово, а 
другой игрок - отгадать его и написать в чат.

![Croc3](https://github.com/klepaski/Games/assets/43060010/3abc93ff-2759-42b4-b306-a8ddc681c066)

Если Крокодил напишет в чат ответ, он автоматически зацензурится.
При вводе другим игроком правильного ответа, игра заканчивается.

![Croc4](https://github.com/klepaski/Games/assets/43060010/2326207d-dc2b-4e75-9b64-120bdd8f6ed1)

<h3>ВИСЕЛИЦА</h3>
Ведущий (Host) загадывает слово и подсказку к нему.

![hang0](https://github.com/klepaski/Games/assets/43060010/5078b0ef-1905-4be1-bbfe-05c7f3195cb6)

Другой игрок пытается угадать буквы этого слова. 
Также он может увидеть подсказку.

![hang1](https://github.com/klepaski/Games/assets/43060010/b8fca4d3-fd01-43bf-b3f5-5a78621927e3)

При неверных ходах дорировываются части виселицы.
На странице ведущего динамически отображается тот же экран, что и у игрока, но сам он не может делать ходы.
Игра заканчивается, когда виселица дорисована или когда слово угадано.