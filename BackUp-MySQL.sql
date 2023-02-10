-- phpMyAdmin SQL Dump
-- version 4.7.1
-- https://www.phpmyadmin.net/
--
-- Хост: sql7.freemysqlhosting.net
-- Время создания: Фев 10 2023 г., 16:34
-- Версия сервера: 5.5.62-0ubuntu0.14.04.1
-- Версия PHP: 7.0.33-0ubuntu0.16.04.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `sql7595642`
--

-- --------------------------------------------------------

--
-- Структура таблицы `additional_information`
--

CREATE TABLE `additional_information` (
  `id_add_info` int(11) NOT NULL,
  `add_information` varchar(50) NOT NULL,
  `specialization` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `additional_information`
--

INSERT INTO `additional_information` (`id_add_info`, `add_information`, `specialization`) VALUES
(1, 'Не опеределено', 'Не опеределено'),
(2, 'Разработка приложений', 'Программист'),
(3, 'Музыка', 'Музыкант'),
(4, 'Изобразительное искусство', 'Художник'),
(5, 'Юный физик', 'Ученый'),
(6, 'Юный трудовик', 'Технолог'),
(7, 'Юный химик', 'Врач'),
(8, 'Юный биолог', 'Ветеренар'),
(9, 'Юный Стихотворец', 'Писатель'),
(10, 'Легкая атлетика', 'Атлетист');

-- --------------------------------------------------------

--
-- Структура таблицы `hobbies`
--

CREATE TABLE `hobbies` (
  `id` int(11) NOT NULL,
  `hobbies` varchar(50) NOT NULL,
  `id_add_information` int(11) DEFAULT NULL,
  `id_full_name` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `hobbies`
--

INSERT INTO `hobbies` (`id`, `hobbies`, `id_add_information`, `id_full_name`) VALUES
(1, 'Программирование', 2, 1),
(2, 'Рисование', 4, 2),
(3, 'Музыка', 3, 3),
(4, 'Химия', 7, 4),
(5, 'Писать стихи', 9, 5),
(6, 'Биология', 8, 6),
(7, 'Рукоделие', 6, 7),
(8, 'Физика', 5, 8),
(9, 'Спорт', 10, 9),
(10, 'Музыка', 4, 10);

-- --------------------------------------------------------

--
-- Структура таблицы `members`
--

CREATE TABLE `members` (
  `id` int(11) NOT NULL,
  `full_name` varchar(50) NOT NULL,
  `class` varchar(50) NOT NULL,
  `birth_year` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `members`
--

INSERT INTO `members` (`id`, `full_name`, `class`, `birth_year`) VALUES
(1, 'Леденев Кирилл Михайлович', '11', '2004-06-11'),
(2, 'Петровская Василиса Кирилловна', '5', '2000-01-01'),
(3, 'Широков Андрей Филиппович', '6', '2000-01-01'),
(4, 'Рожков Денис Богдадович', '7', '2000-01-01'),
(5, 'Матвеева Амелия Львовна', '5', '2000-01-01'),
(6, 'Гусев Артем Глебович', '9', '2000-01-01'),
(7, 'Климов Федор Семенович', '6', '2000-01-01'),
(8, 'Лукьянова Ульяна Артемовна', '9', '2000-01-01'),
(9, 'Наумова Полина Павловна', '5', '2000-01-01'),
(10, 'Калинин Егор Дмитриевич', '5', '2000-01-01');

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `permission` varchar(50) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`id`, `permission`, `username`, `password`) VALUES
(1, 'admin', 'admin', 'admin'),
(2, 'member', 'user', 'user');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `additional_information`
--
ALTER TABLE `additional_information`
  ADD PRIMARY KEY (`id_add_info`);

--
-- Индексы таблицы `hobbies`
--
ALTER TABLE `hobbies`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_add_information` (`id_add_information`),
  ADD KEY `id_full_name` (`id_full_name`);

--
-- Индексы таблицы `members`
--
ALTER TABLE `members`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `additional_information`
--
ALTER TABLE `additional_information`
  MODIFY `id_add_info` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT для таблицы `hobbies`
--
ALTER TABLE `hobbies`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT для таблицы `members`
--
ALTER TABLE `members`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `hobbies`
--
ALTER TABLE `hobbies`
  ADD CONSTRAINT `hobbies_ibfk_1` FOREIGN KEY (`id_full_name`) REFERENCES `members` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `hobbies_ibfk_2` FOREIGN KEY (`id_add_information`) REFERENCES `additional_information` (`id_add_info`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
