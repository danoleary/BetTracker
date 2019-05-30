module AddBookieHandler

open Domain

let handleAddBookie state addBookie =
    BookieAdded addBookie