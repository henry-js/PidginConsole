last(cons(X, nil), X).
last(cons(X, Xs), Y) :- last(Xs, Y).
