dialog = {
    confirm: message => window.confirm(message),
    alert: message => { window.alert(message); return true; }
}

export dialog;