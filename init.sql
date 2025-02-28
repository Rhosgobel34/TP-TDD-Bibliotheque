CREATE USER 'api-biblio'@'%' IDENTIFIED BY 'bibliothecaire';
GRANT ALL PRIVILEGES ON bibliotheque.* TO 'api-biblio'@'%';
FLUSH PRIVILEGES;

CREATE TABLE editor (
    id INT PRIMARY KEY AUTO_INCREMENT,
    editor_name VARCHAR(255) NOT NULL
);

CREATE TABLE civility (
    id INT PRIMARY KEY AUTO_INCREMENT,
    label VARCHAR(255) NOT NULL
);

CREATE TABLE author (
    id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL
);

CREATE TABLE book (
    id INT PRIMARY KEY AUTO_INCREMENT,
    isbn VARCHAR(13) UNIQUE NOT NULL,
    title VARCHAR(255) NOT NULL,
    available BOOLEAN NOT NULL DEFAULT TRUE,
    editor_id INT NOT NULL,
    FOREIGN KEY (editor_id) REFERENCES editor(id) ON DELETE CASCADE
);

CREATE TABLE book_format (
    id INT PRIMARY KEY AUTO_INCREMENT,
    label VARCHAR(255) NOT NULL
);

CREATE TABLE subscriber (
    id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(255) UNIQUE NOT NULL,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    birth_date DATE NOT NULL,
    email VARCHAR(255) NOT NULL,
    civility_id INT,
    FOREIGN KEY (civility_id) REFERENCES civility(id) ON DELETE SET NULL
);

CREATE TABLE status (
    id INT PRIMARY KEY AUTO_INCREMENT,
    label VARCHAR(255) NOT NULL
);

CREATE TABLE reservation (
    id INT PRIMARY KEY AUTO_INCREMENT,
    start_date DATE NOT NULL,
    finished BOOLEAN NOT NULL DEFAULT FALSE,
    subscriber_id INT NOT NULL,
    FOREIGN KEY (subscriber_id) REFERENCES subscriber(id) ON DELETE CASCADE
);

CREATE TABLE write_relation (
    author_id INT,
    book_id INT,
    PRIMARY KEY (author_id, book_id),
    FOREIGN KEY (author_id) REFERENCES author(id) ON DELETE CASCADE,
    FOREIGN KEY (book_id) REFERENCES book(id) ON DELETE CASCADE
);

CREATE TABLE embody (
    book_id INT,
    format_id INT,
    PRIMARY KEY (book_id, format_id),
    FOREIGN KEY (book_id) REFERENCES book(id) ON DELETE CASCADE,
    FOREIGN KEY (format_id) REFERENCES book_format(id) ON DELETE CASCADE
);

CREATE TABLE be (
    status_id INT,
    reservation_id INT,
    PRIMARY KEY (status_id, reservation_id),
    FOREIGN KEY (status_id) REFERENCES status(id) ON DELETE CASCADE,
    FOREIGN KEY (reservation_id) REFERENCES reservation(id) ON DELETE CASCADE
);

CREATE TABLE reserved (
    book_id INT,
    reservation_id INT,
    PRIMARY KEY (book_id, reservation_id),
    FOREIGN KEY (book_id) REFERENCES book(id) ON DELETE CASCADE,
    FOREIGN KEY (reservation_id) REFERENCES reservation(id) ON DELETE CASCADE
);

CREATE TABLE borrow (
    subscriber_id INT,
    reservation_id INT,
    PRIMARY KEY (subscriber_id, reservation_id),
    FOREIGN KEY (subscriber_id) REFERENCES subscriber(id) ON DELETE CASCADE,
    FOREIGN KEY (reservation_id) REFERENCES reservation(id) ON DELETE CASCADE
);

CREATE TABLE edit (
    editor_id INT,
    book_id INT,
    PRIMARY KEY (editor_id, book_id),
    FOREIGN KEY (editor_id) REFERENCES editor(id) ON DELETE CASCADE,
    FOREIGN KEY (book_id) REFERENCES book(id) ON DELETE CASCADE
);

CREATE TABLE personify (
    subscriber_id INT,
    civility_id INT,
    PRIMARY KEY (subscriber_id, civility_id),
    FOREIGN KEY (subscriber_id) REFERENCES subscriber(id) ON DELETE CASCADE,
    FOREIGN KEY (civility_id) REFERENCES civility(id) ON DELETE CASCADE
);