(function () {
    let pendingForm = null;

    document.addEventListener('submit', function (event) {
        const form = event.target;
        if (!form.matches('[data-confirm-submit="true"]')) return;

        event.preventDefault();
        pendingForm = form;

        const title = form.getAttribute('data-confirm-title') || 'Confirmar acción';
        const message = form.getAttribute('data-confirm-message') || '¿Deseas continuar?';
        const buttonText = form.getAttribute('data-confirm-button') || 'Sí, continuar';

        document.getElementById('appConfirmTitle').textContent = title;
        document.getElementById('appConfirmMessage').textContent = message;
        document.getElementById('appConfirmButton').textContent = buttonText;

        const modal = new bootstrap.Modal(document.getElementById('appConfirmModal'));
        modal.show();
    });

    document.addEventListener('click', function (event) {
        if (event.target.id !== 'appConfirmButton') return;
        if (!pendingForm) return;

        pendingForm.removeAttribute('data-confirm-submit');
        pendingForm.submit();
        pendingForm = null;
    });
})();
