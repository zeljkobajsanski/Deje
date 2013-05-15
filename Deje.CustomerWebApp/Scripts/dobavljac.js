function IdDelatnostiValueChanged() {
    IdVrsteDobavljaca.PerformCallback();
}

function IdVrsteDobavljacaBeginCallback(s, e) {
    e.customArgs['id'] = IdDelatnosti.GetValue();
}

function pronadjiAdresu() {
    pronadjiLokaciju(Mesto.GetValue() + ' ' + Adresa.GetValue());
}

