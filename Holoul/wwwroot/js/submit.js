document.addEventListener('DOMContentLoaded', function () {
    // DOM Elements
    const problemInput = document.getElementById('problem-input');
    const clearButton = document.getElementById('clear-button');
    const fileInput = document.getElementById('file-input');
    const fileButton = document.getElementById('file-button');
    const dropZone = document.getElementById('drop-zone');
    const filePreview = document.getElementById('file-preview');
    const fileList = document.getElementById('file-list');
    const submitButton = document.getElementById('submit-button');
    const solutionArea = document.getElementById('solution-area');
    const solutionText = document.getElementById('solution-text');
    const loadingIndicator = document.getElementById('loading');

    // State
    let files = [];

    // Event Listeners
    problemInput.addEventListener('input', handleProblemInput);
    clearButton.addEventListener('click', clearProblem);
    fileButton.addEventListener('click', () => fileInput.click());
    fileInput.addEventListener('change', handleFileUpload);
    submitButton.addEventListener('click', handleSubmit);

    // Drag and Drop
    dropZone.addEventListener('dragover', handleDragOver);
    dropZone.addEventListener('dragleave', handleDragLeave);
    dropZone.addEventListener('drop', handleDrop);
    submitButton.addEventListener('click', createRipple);

    // Auto-resize textarea
    problemInput.addEventListener('input', function () {
        this.style.height = 'auto';
        this.style.height = (this.scrollHeight) + 'px';
    });

    // Functions
    function handleProblemInput(e) {
        const value = e.target.value.trim();

        if (value) {
            clearButton.classList.remove('hidden');
            submitButton.disabled = false;
        } else {
            clearButton.classList.add('hidden');
            submitButton.disabled = true;
        }

        // Auto resize the textarea
        e.target.style.height = 'auto';
        e.target.style.height = (e.target.scrollHeight) + 'px';
    }

    function clearProblem() {
        problemInput.value = '';
        problemInput.style.height = 'auto';
        clearButton.classList.add('hidden');
        submitButton.disabled = true;
        files = [];
        renderFileList();
        problemInput.focus();
    }

    function handleFileUpload(e) {
        if (e.target.files) {
            const newFiles = Array.from(e.target.files);
            files = [...files, ...newFiles];
            renderFileList();
        }
    }

    function handleDragOver(e) {
        e.preventDefault();
        dropZone.classList.add('drag-active');
        document.querySelector('.upload-text').textContent = 'Drop files here';
    }

    function handleDragLeave() {
        dropZone.classList.remove('drag-active');
        document.querySelector('.upload-text').textContent = 'Drag & drop or click to attach files';
    }

    function handleDrop(e) {
        e.preventDefault();
        dropZone.classList.remove('drag-active');
        document.querySelector('.upload-text').textContent = 'Drag & drop or click to attach files';

        if (e.dataTransfer.files) {
            const newFiles = Array.from(e.dataTransfer.files);
            files = [...files, ...newFiles];
            renderFileList();
        }
    }

    function renderFileList() {
        if (files.length > 0) {
            filePreview.classList.remove('hidden');
            fileList.innerHTML = '';

            files.forEach((file, index) => {
                const fileItem = document.createElement('div');
                fileItem.className = 'file-item';

                const icon = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
                icon.setAttribute('width', '14');
                icon.setAttribute('height', '14');
                icon.setAttribute('viewBox', '0 0 24 24');
                icon.setAttribute('fill', 'none');
                icon.setAttribute('stroke', 'currentColor');
                icon.setAttribute('stroke-width', '2');
                icon.setAttribute('stroke-linecap', 'round');
                icon.setAttribute('stroke-linejoin', 'round');

                if (file.type.startsWith('image/')) {
                    icon.innerHTML = '<rect x="3" y="3" width="18" height="18" rx="2" ry="2"></rect><circle cx="8.5" cy="8.5" r="1.5"></circle><polyline points="21 15 16 10 5 21"></polyline>';
                } else {
                    icon.innerHTML = '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path><polyline points="14 2 14 8 20 8"></polyline><line x1="16" y1="13" x2="8" y2="13"></line><line x1="16" y1="17" x2="8" y2="17"></line><polyline points="10 9 9 9 8 9"></polyline>';
                }

                const span = document.createElement('span');
                span.textContent = file.name;

                const removeButton = document.createElement('button');
                removeButton.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>';
                removeButton.addEventListener('click', () => removeFile(index));

                fileItem.appendChild(icon);
                fileItem.appendChild(span);
                fileItem.appendChild(removeButton);
                fileList.appendChild(fileItem);
            });
        } else {
            filePreview.classList.add('hidden');
        }
    }

    function removeFile(index) {
        files = files.filter((_, i) => i !== index);
        renderFileList();
    }

    function createRipple(e) {
        if (submitButton.disabled) return;

        const button = submitButton;
        const ripple = document.createElement('span');
        const diameter = Math.max(button.clientWidth, button.clientHeight);
        const radius = diameter / 2;

        ripple.style.width = ripple.style.height = `${diameter}px`;
        ripple.style.left = `${e.clientX - button.getBoundingClientRect().left - radius}px`;
        ripple.style.top = `${e.clientY - button.getBoundingClientRect().top - radius}px`;
        ripple.classList.add('ripple');

        const existingRipple = button.querySelector('.ripple');
        if (existingRipple) {
            existingRipple.remove();
        }

        button.appendChild(ripple);

        setTimeout(() => {
            ripple.remove();
        }, 600);
    }

    function handleSubmit() {
        if (!problemInput.value.trim()) return;

        // Show loading state
        solutionArea.classList.remove('empty');
        solutionArea.classList.add('filled');
        loadingIndicator.classList.remove('hidden');
        document.getElementById('solution-content').classList.add('hidden');

        // Disable submit button
        submitButton.disabled = true;

        // Simulate API call with setTimeout
        setTimeout(() => {
            const responses = [
                "To fix a leaky faucet, first turn off the water supply. Then, disassemble the faucet by removing the handle and cartridge. Look for worn-out washers, O-rings, or seals, which are the most common causes of leaks. Replace any damaged parts, reassemble, and turn the water back on to test.",
                "For a clogged drain, try using a mixture of baking soda and vinegar. Pour half a cup of baking soda followed by half a cup of vinegar down the drain. Cover with a wet cloth, wait 5-10 minutes, then flush with hot water. For stubborn clogs, a plunger or drain snake may be necessary.",
                "To fix a running toilet, check the flapper valve at the bottom of the tank. If it's not sealing properly, replace it. Also inspect the fill valve and float to ensure they're working correctly. These parts are inexpensive and relatively easy to replace with basic tools.",
                "For squeaky hardwood floors, try sprinkling talcum powder or powdered graphite between the boards and sweep it into the cracks. For a more permanent solution, secure loose boards with finish nails or screws specifically designed for hardwood flooring. Be careful not to damage the wood.",
                "To address wall cracks, first determine if they're merely cosmetic or structural. For small, cosmetic cracks, clean the area, apply spackle with a putty knife, let it dry, sand it smooth, and then paint. For larger or recurring cracks, use mesh tape before applying joint compound. Structural cracks may require professional assessment."
            ];

            const randomResponse = responses[Math.floor(Math.random() * responses.length)];

            // Hide loading, show content
            loadingIndicator.classList.add('hidden');
            document.getElementById('solution-content').classList.remove('hidden');

            // Simulate typing effect
            typeWriterEffect(solutionText, randomResponse, 0);

            // Re-enable submit button
            submitButton.disabled = false;
        }, 2000);
    }

    function typeWriterEffect(element, text, index) {
        if (index < text.length) {
            element.textContent = text.substring(0, index + 1);
            setTimeout(() => typeWriterEffect(element, text, index + 1), 15);
        }
    }
});