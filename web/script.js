document.getElementById("mailForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const form = e.target;
    const data = new FormData();

    data.append("to", form.to.value);
    data.append("subject", form.subject.value);
    data.append("body", form.body.value);

    if (form.cv.files.length)
        data.append("file", form.cv.files[0]);

    try {
        const res = await fetch("http://localhost:5001/create-drafts", {
            method: "POST",
            body: data
        });

        const json = await res.json();
        alert("נוצרו טיוטות: " + json.count);
    } catch (err) {
        alert("שגיאה: " + err);
    }
});