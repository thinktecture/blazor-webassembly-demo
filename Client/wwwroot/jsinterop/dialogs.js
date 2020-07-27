var interopJS = interopJS || {}

interopJS.dialogs = {
    confirm: message => window.confirm(message),
    alert: message => { window.alert(message); return true; }
}

window.confToolInterop = interopJS;