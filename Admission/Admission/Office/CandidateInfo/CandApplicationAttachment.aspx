<%@ Page Title="Application Form - Attachment" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationAttachment.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationAttachment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />

    <!-- Cropper.js Library -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/cropperjs/1.6.1/cropper.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/cropperjs/1.6.1/cropper.min.js"></script>

    <style>
         :root {
            --primary: #091B3F;
            --secondary: #1E3A8A;
            --accent: #3B82F6;
            --accent-light: #93C5FD;
            --success: #059669;
            --warning: #D97706;
            --danger: #DC2626;
            --light: #F9FAFB;
            --dark: #111827;
            --gray: #6B7280;
            --bg-gradient: linear-gradient(145deg, var(--primary), var(--secondary));
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            --shadow-sm: 0 2px 4px rgba(0,0,0,0.12), 0 2px 3px rgba(0,0,0,0.24);
            --shadow-md: 0 6px 12px rgba(0,0,0,0.1);
            --shadow-lg: 0 12px 30px rgba(0,0,0,0.12);
            --radius-sm: 6px;
            --radius-md: 10px;
            --radius-lg: 18px;
        }


        /* Admin Container */
        .admin-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-lg);
            padding: 2rem;
            margin-bottom: 2.5rem;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
        }

        /* Progress Navigation */
        .progress-nav {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            padding: 3.5rem;
            margin-bottom: 1.5rem;
            box-shadow: var(--shadow-lg);
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border: 1px solid rgba(255, 255, 255, 0.2);
            position: relative;
            overflow: hidden;
        }

            .progress-nav::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 50%, var(--accent) 100%);
                opacity: 0.05;
                z-index: 0;
            }

        .breadcrumb-modern {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin: 0;
            padding: 0;
            list-style: none;
            position: relative;
            z-index: 1;
            flex-wrap: wrap;
            gap: 1rem;
        }

            .breadcrumb-modern::before {
                content: '';
                position: absolute;
                top: 50%;
                left: 0;
                right: 0;
                height: 4px;
                background: linear-gradient(to right, #e2e8f0, var(--accent-light), #e2e8f0);
                border-radius: 2px;
                z-index: -1;
            }

            .breadcrumb-modern li {
                position: relative;
                display: flex;
                align-items: center;
                justify-content: center;
                min-width: 65px;
                min-height: 65px;
                transition: var(--transition);
                z-index: 2;
            }

                .breadcrumb-modern li a {
                    position: relative;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    width: 65px;
                    height: 65px;
                    background: white;
                    border-radius: 50%;
                    border: 4px solid #e2e8f0;
                    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.12);
                    text-decoration: none;
                    transition: var(--transition);
                    cursor: pointer;
                    color: var(--gray);
                    font-weight: 700;
                    font-size: 1.5rem;
                }

                    .breadcrumb-modern li a::before {
                        content: attr(data-step);
                        position: absolute;
                        top: 50%;
                        left: 50%;
                        transform: translate(-50%, -50%);
                        width: 32px;
                        height: 32px;
                        border-radius: 50%;
                        background: #e2e8f0;
                        color: var(--gray);
                        font-weight: 700;
                        font-size: 1.5rem;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        transition: var(--transition);
                    }

                    .breadcrumb-modern li a::after {
                        content: attr(data-label);
                        position: absolute;
                        top: 100%;
                        left: 50%;
                        transform: translateX(-50%);
                        color: var(--dark);
                        font-weight: 600;
                        font-size: 1.2rem;
                        white-space: nowrap;
                        margin-top: 0.75rem;
                        transition: var(--transition);
                        text-align: center;
                        max-width: 90px;
                        overflow: hidden;
                        text-overflow: ellipsis;
                    }

                .breadcrumb-modern li.active a {
                    border-color: var(--accent);
                    transform: scale(1.15);
                    box-shadow: 0 8px 24px rgba(59, 130, 246, 0.35);
                }

                    .breadcrumb-modern li.active a::before {
                        background: var(--accent);
                        color: white;
                    }

                    .breadcrumb-modern li.active a::after {
                        color: var(--accent);
                        font-weight: 700;
                    }

                .breadcrumb-modern li:not(.active):hover a {
                    transform: scale(1.08);
                    border-color: var(--accent-light);
                }

                    .breadcrumb-modern li:not(.active):hover a::before {
                        background: var(--accent-light);
                        color: white;
                    }
        /* ===== BASE STYLES ===== */
        .application-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            padding: 1rem;
            margin-bottom: 2rem;
        }

        .panel {
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            border: none;
            overflow: hidden;
            margin-bottom: 30px;
        }

        .panel-heading {
            background-color: #2c3e50;
            color: white;
            padding: 18px 25px;
            border-bottom: none;
            position: relative;
        }

        .panel-title {
            font-weight: 600;
            font-size: 20px;
            margin: 0;
            display: flex;
            align-items: center;
        }

        .required-notice {
            font-size: 14px;
            color: #ffcc00;
            margin-top: 8px;
            font-weight: 500;
        }

        .panel-body {
            padding: 25px;
        }

        /* ===== REQUIREMENTS SECTION ===== */
        .requirements-box {
            background: #f8f9fa;
            border-radius: 8px;
            margin-bottom: 30px;
            border: 1px solid #e9ecef;
            overflow: hidden;
        }

        .requirement-header {
            background: #2c3e50;
            color: white;
            padding: 15px 20px;
            font-weight: 600;
            font-size: 16px;
            display: flex;
            align-items: center;
        }

            .requirement-header i {
                margin-right: 12px;
                font-size: 18px;
            }

        .requirement-content {
            padding: 20px 25px;
        }

        .requirement-column {
            padding: 0 15px;
        }

            .requirement-column h4 {
                color: #2c3e50;
                font-weight: 600;
                margin-bottom: 18px;
                display: flex;
                align-items: center;
                font-size: 16px;
            }

                .requirement-column h4 i {
                    margin-right: 12px;
                    font-size: 20px;
                }

            .requirement-column ul {
                list-style: none;
                padding: 0;
                margin: 0;
            }

            .requirement-column li {
                margin-bottom: 12px;
                display: flex;
                align-items: flex-start;
                line-height: 1.5;
                font-size: 14px;
            }

                .requirement-column li i {
                    margin-right: 12px;
                    color: #27ae60;
                    font-size: 16px;
                    flex-shrink: 0;
                    margin-top: 2px;
                }

        .photo-requirements {
            border-right: 1px solid #e9ecef;
        }

        /* ===== UPLOAD SECTIONS ===== */
        .required-upload {
            padding: 0 12px;
        }

        .upload-section {
            border: 2px solid #3498db;
            border-radius: 8px;
            margin-bottom: 25px;
            background: white;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
            overflow: hidden;
        }

        .section-header {
            background: linear-gradient(to right, #e3f2fd, #fff);
            padding: 16px 20px;
            border-bottom: 1px solid #f0f0f0;
            font-weight: 600;
            color: #2c3e50;
            display: flex;
            align-items: center;
            font-size: 16px;
        }

            .section-header i {
                margin-right: 12px;
                color: #3498db;
                font-size: 20px;
            }

        .required-badge {
            background: #3498db;
            color: white;
            font-size: 12px;
            padding: 4px 10px;
            border-radius: 12px;
            margin-left: 12px;
            font-weight: 500;
        }

        .section-body {
            padding: 20px;
        }

        /* ===== UPLOAD AREAS ===== */
        .upload-area {
            border: 2px dashed #3498db;
            border-radius: 8px;
            padding: 30px 20px;
            text-align: center;
            background: rgba(52, 152, 219, 0.03);
            cursor: pointer;
            margin-bottom: 20px;
            transition: all 0.3s ease;
        }

            .upload-area:hover {
                background: rgba(52, 152, 219, 0.08);
                border-color: #2980b9;
                transform: translateY(-2px);
            }

            .upload-area.dragover {
                background: rgba(52, 152, 219, 0.15);
                border-color: #2980b9;
            }

        .upload-icon {
            font-size: 40px;
            color: #3498db;
            margin-bottom: 15px;
        }

        .upload-text {
            font-weight: 600;
            margin-bottom: 8px;
            color: #2c3e50;
            font-size: 16px;
        }

        .upload-subtext {
            color: #7f8c8d;
            font-size: 13px;
            margin-bottom: 5px;
        }

        .upload-feature {
            margin-top: 20px;
            padding-top: 15px;
            border-top: 1px dashed #e0e0e0;
            color: #3498db;
            font-weight: 600;
            font-size: 14px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .upload-feature i {
                margin-right: 10px;
                font-size: 16px;
            }

        /* ===== PHOTO CROPPER STYLES ===== */
        .cropper-container {
            margin: 20px 0;
            background: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            border: 1px solid #e9ecef;
        }

        .cropper-wrapper {
            max-width: 100%;
            max-height: 400px;
            margin: 0 auto;
            position: relative;
        }

        .crop-image {
            max-width: 100%;
            display: block;
        }

        .cropper-preview-container {
            margin-top: 20px;
            text-align: center;
        }

            .cropper-preview-container h5 {
                color: #2c3e50;
                font-weight: 600;
                margin-bottom: 10px;
            }

        .cropper-preview {
            width: 150px;
            height: 150px;
            border: 2px solid #ddd;
            border-radius: 8px;
            margin: 0 auto;
            overflow: hidden;
            background: #f5f5f5;
        }

        .signature-preview {
            width: 300px;
            height: 80px;
        }

        .cropper-controls {
            display: flex;
            justify-content: center;
            gap: 10px;
            margin-top: 15px;
            flex-wrap: wrap;
        }

        .crop-btn {
            padding: 8px 16px;
            border: none;
            border-radius: 5px;
            font-size: 14px;
            cursor: pointer;
            transition: all 0.3s;
            font-weight: 500;
        }

        .crop-btn-primary {
            background: #3498db;
            color: white;
        }

            .crop-btn-primary:hover {
                background: #2980b9;
            }

        .crop-btn-secondary {
            background: #95a5a6;
            color: white;
        }

            .crop-btn-secondary:hover {
                background: #7f8c8d;
            }

        .crop-btn-success {
            background: #27ae60;
            color: white;
        }

            .crop-btn-success:hover {
                background: #219955;
            }

        .crop-btn-danger {
            background: #e74c3c;
            color: white;
        }

            .crop-btn-danger:hover {
                background: #c0392b;
            }

        /* ===== BUTTONS ===== */
        .btn-upload-final {
            background: #27ae60;
            color: white;
            border: none;
            padding: 12px 28px;
            border-radius: 6px;
            font-weight: 600;
            font-size: 14px;
            transition: all 0.3s;
            display: inline-flex;
            align-items: center;
            justify-content: center;
        }

            .btn-upload-final:hover {
                background: #219955;
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }

            .btn-upload-final i {
                margin-right: 8px;
            }

        /* ===== CURRENT FILE DISPLAY ===== */
        .current-file {
            text-align: center;
            margin: 25px 0;
            padding: 20px;
            background: #f9f9f9;
            border-radius: 8px;
            border: 1px solid #eee;
        }

        .current-file-header {
            font-weight: 600;
            margin-bottom: 15px;
            color: #2c3e50;
            font-size: 15px;
        }

        .current-image {
            width: 160px;
            height: 160px;
            border: 1px solid #ddd;
            border-radius: 8px;
            object-fit: cover;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        }

        .current-signature {
            width: 280px;
            height: 120px;
            border: 1px solid #ddd;
            border-radius: 8px;
            object-fit: contain;
            background: white;
            padding: 15px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        }

        /* ===== LOADING SPINNER ===== */
        .loading-spinner {
            display: none;
            text-align: center;
            padding: 25px;
            background: rgba(255, 255, 255, 0.9);
            border-radius: 8px;
            margin: 15px 0;
        }

            .loading-spinner.show {
                display: block;
            }

        .spinner {
            width: 50px;
            height: 50px;
            border: 4px solid rgba(52, 152, 219, 0.2);
            border-top: 4px solid #3498db;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: 0 auto 15px;
        }

        .spinner-text {
            font-weight: 600;
            margin-bottom: 5px;
            color: #2c3e50;
        }

        .spinner-subtext {
            color: #7f8c8d;
            font-size: 13px;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        /* ===== HIDDEN CLASS ===== */
        .hidden {
            display: none !important;
        }

        /* ===== MESSAGE STYLES ===== */
        .message-success,
        .message-error,
        .message-warning,
        .message-info {
            padding: 14px 18px;
            border-radius: 10px;
            font-size: 14px;
            font-weight: 500;
            line-height: 1.5;
            margin-bottom: 10px;
            border: none;
            position: relative;
            display: flex;
            align-items: center;
            gap: 12px;
            animation: messageSlide 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94);
        }

        .message-success {
            background: rgba(46, 204, 113, 0.1);
            color: #27ae60;
            border-left: 4px solid #27ae60;
        }

            .message-success::before {
                content: "✓";
                font-size: 18px;
                font-weight: 700;
                color: #27ae60;
                width: 20px;
                height: 20px;
                display: flex;
                align-items: center;
                justify-content: center;
                background: rgba(39, 174, 96, 0.15);
                border-radius: 50%;
                flex-shrink: 0;
            }

        .message-error {
            background: rgba(231, 76, 60, 0.1);
            color: #e74c3c;
            border-left: 4px solid #e74c3c;
        }

            .message-error::before {
                content: "×";
                font-size: 20px;
                font-weight: 700;
                color: #e74c3c;
                width: 20px;
                height: 20px;
                display: flex;
                align-items: center;
                justify-content: center;
                background: rgba(231, 76, 60, 0.15);
                border-radius: 50%;
                flex-shrink: 0;
            }

        .message-info {
            background: rgba(52, 152, 219, 0.1);
            color: #3498db;
            border-left: 4px solid #3498db;
        }

            .message-info::before {
                content: "i";
                font-size: 14px;
                font-weight: 700;
                color: #3498db;
                width: 20px;
                height: 20px;
                display: flex;
                align-items: center;
                justify-content: center;
                background: rgba(52, 152, 219, 0.15);
                border-radius: 50%;
                flex-shrink: 0;
            }

        @keyframes messageSlide {
            from {
                opacity: 0;
                transform: translateY(-10px) scale(0.95);
            }

            to {
                opacity: 1;
                transform: translateY(0) scale(1);
            }
        }

        /* ===== RESPONSIVE ADJUSTMENTS ===== */
        @media (max-width: 992px) {
            .panel-body {
                padding: 20px;
            }

            .requirement-content {
                padding: 15px;
            }

            .section-body {
                padding: 15px;
            }

            .cropper-wrapper {
                max-height: 300px;
            }
        }

        @media (max-width: 768px) {
            .photo-requirements {
                border-right: none;
                border-bottom: 1px solid #e9ecef;
                padding-bottom: 20px;
                margin-bottom: 20px;
            }

            .required-upload {
                margin-bottom: 30px;
            }

            .upload-section {
                margin-bottom: 20px;
            }

            .cropper-controls {
                flex-direction: column;
                align-items: center;
            }

            .crop-btn {
                width: 120px;
            }
        }

        @media (max-width: 576px) {
            .panel-heading {
                padding: 15px;
            }

            .requirement-header {
                padding: 12px 15px;
                font-size: 15px;
            }

            .section-header {
                padding: 12px 15px;
                font-size: 15px;
            }

            .upload-area {
                padding: 25px 15px;
            }

            .current-image {
                width: 140px;
                height: 140px;
            }

            .current-signature {
                width: 240px;
                height: 100px;
            }

            .cropper-container {
                padding: 15px;
            }

            .cropper-wrapper {
                max-height: 250px;
            }
        }
                @media (max-width: 768px) {
            .admin-container {
                padding: 1.5rem;
            }

            .basic-section {
                padding: 2rem;
            }

            .form-table-modern td {
                padding: 1rem;
                display: block;
                width: 100% !important;
                font-size: 1rem;
            }

                .form-table-modern td:first-child {
                    width: 100% !important;
                    font-weight: 700;
                    border-bottom: none;
                    padding-bottom: 0.5rem;
                    background: transparent;
                    font-size: 1.4rem;
                }

            .progress-nav {
                padding: 2.5rem;
                overflow-x: auto;
            }

            .breadcrumb-modern {
                flex-wrap: nowrap;
                gap: 0.75rem;
                min-width: max-content;
            }

                .breadcrumb-modern li {
                    min-width: 55px;
                    min-height: 55px;
                }

                    .breadcrumb-modern li a {
                        width: 55px;
                        height: 55px;
                    }

                        .breadcrumb-modern li a::before {
                            width: 28px;
                            height: 28px;
                            font-size: 1.2rem;
                        }

                        .breadcrumb-modern li a::after {
                            font-size: 1rem;
                            max-width: 70px;
                        }

            .quota-info-grid {
                grid-template-columns: 1fr;
            }

            .basic-section h4 {
                font-size: 1.25rem;
            }

            .panel-header-modern {
                padding: 1.5rem 1.75rem;
                font-size: 1.2rem;
            }

            .panel-body-modern {
                padding: 2rem;
            }

            .btn-modern {
                padding: 0.875rem 1.75rem;
                font-size: 1rem;
            }
        }

        @media (max-width: 576px) {
            body {
                font-size: 14px;
            }

            .logo-container img {
                max-height: 75px;
            }

            .admin-container {
                padding: 1rem;
            }

            .basic-section {
                padding: 1.5rem;
            }

            .breadcrumb-modern li {
                min-width: 50px;
                min-height: 50px;
            }

                .breadcrumb-modern li a {
                    width: 50px;
                    height: 50px;
                    border-width: 3px;
                }

                    .breadcrumb-modern li a::before {
                        width: 24px;
                        height: 24px;
                        font-size: 1.5rem;
                    }

                    .breadcrumb-modern li a::after {
                        font-size: 1rem;
                        max-width: 60px;
                    }

            .panel-body-modern {
                padding: 1.5rem;
            }

            .btn-modern {
                padding: 0.75rem 1.5rem;
                font-size: 0.95rem;
                width: 100%;
                justify-content: center;
            }
        }
    </style>

    <script>
        let originalImageFile = null;
        let cropper = null;
        let croppedImageBlob = null;

        // Signature cropping variables
        let originalSignatureFile = null;
        let signatureCropper = null;
        let croppedSignatureBlob = null;

        // Initialize when page loads
        document.addEventListener('DOMContentLoaded', function () {
            setupEventListeners();
        });

        function setupEventListeners() {
            const uploadArea = document.getElementById('photoUploadArea');
            const fileInput = document.getElementById('<%= FileUploadPhoto.ClientID %>');

            // Signature upload elements
            const signatureUploadArea = document.getElementById('signatureUploadArea');
            const signatureFileInput = document.getElementById('<%= FileUploadSignature.ClientID %>');

            if (uploadArea && fileInput) {
                // Photo drag and drop
                uploadArea.addEventListener('click', () => fileInput.click());
                uploadArea.addEventListener('dragover', handleDragOver);
                uploadArea.addEventListener('dragleave', handleDragLeave);
                uploadArea.addEventListener('drop', handleDrop);

                // File input change
                fileInput.addEventListener('change', handleFileSelect);
            }

            if (signatureUploadArea && signatureFileInput) {
                // Signature drag and drop
                signatureUploadArea.addEventListener('click', () => signatureFileInput.click());
                signatureUploadArea.addEventListener('dragover', handleSignatureDragOver);
                signatureUploadArea.addEventListener('dragleave', handleSignatureDragLeave);
                signatureUploadArea.addEventListener('drop', handleSignatureDrop);

                // Signature file input change
                signatureFileInput.addEventListener('change', handleSignatureFileSelect);
            }
        }

        function handleDragOver(e) {
            e.preventDefault();
            document.getElementById('photoUploadArea').classList.add('dragover');
        }

        function handleDragLeave(e) {
            e.preventDefault();
            document.getElementById('photoUploadArea').classList.remove('dragover');
        }

        function handleDrop(e) {
            e.preventDefault();
            document.getElementById('photoUploadArea').classList.remove('dragover');

            const files = e.dataTransfer.files;
            if (files.length > 0) {
                processDroppedFile(files[0]);
            }
        }

        function handleFileSelect(e) {
            const file = e.target.files[0];
            if (file) {
                processDroppedFile(file);
            }
        }

        // Signature drag and drop handlers
        function handleSignatureDragOver(e) {
            e.preventDefault();
            document.getElementById('signatureUploadArea').classList.add('dragover');
        }

        function handleSignatureDragLeave(e) {
            e.preventDefault();
            document.getElementById('signatureUploadArea').classList.remove('dragover');
        }

        function handleSignatureDrop(e) {
            e.preventDefault();
            document.getElementById('signatureUploadArea').classList.remove('dragover');

            const files = e.dataTransfer.files;
            if (files.length > 0) {
                processDroppedSignatureFile(files[0]);
            }
        }

        function handleSignatureFileSelect(e) {
            const file = e.target.files[0];
            if (file) {
                processDroppedSignatureFile(file);
            }
        }

        function processDroppedFile(file) {
            if (validateFile(file)) {
                originalImageFile = file;
                showImageCropper(file);
            }
        }

        function processDroppedSignatureFile(file) {
            if (validateSignatureFile(file)) {
                originalSignatureFile = file;
                showSignatureCropper(file);
            }
        }

        function validateFile(file) {
            // Validate file type
            const allowedTypes = ['image/jpeg', 'image/jpg'];
            if (!allowedTypes.includes(file.type)) {
                showMessage('Please upload JPG image only', 'error');
                return false;
            }

            // Validate file size (150KB = 153600 bytes)
            if (file.size > 153600) {
                showMessage('File size must be less than 150KB', 'error');
                return false;
            }

            return true;
        }

        function validateSignatureFile(file) {
            // Validate file type
            const allowedTypes = ['image/jpeg', 'image/jpg'];
            if (!allowedTypes.includes(file.type)) {
                showSignatureMessage('Please upload JPG image only', 'error');
                return false;
            }

            // Validate file size (150KB = 153600 bytes)
            if (file.size > 153600) {
                showSignatureMessage('File size must be less than 150KB', 'error');
                return false;
            }

            return true;
        }

        function showImageCropper(file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                // Hide upload step and show cropper
                document.getElementById('uploadStep').classList.add('hidden');
                document.getElementById('cropperStep').classList.remove('hidden');

                // Set image source
                const cropImage = document.getElementById('cropImage');
                cropImage.src = e.target.result;

                // Initialize cropper
                if (cropper) {
                    cropper.destroy();
                }

                cropImage.onload = function () {
                    cropper = new Cropper(cropImage, {
                        aspectRatio: 1, // Square aspect ratio for passport photo
                        viewMode: 1,
                        dragMode: 'move',
                        autoCropArea: 0.8,
                        restore: false,
                        guides: true,
                        center: true,
                        highlight: false,
                        cropBoxMovable: true,
                        cropBoxResizable: true,
                        toggleDragModeOnDblclick: false,
                        minContainerWidth: 300,
                        minContainerHeight: 300,
                        preview: '#cropperPreview'
                    });

                    showMessage('Position and resize the crop area to frame your photo perfectly', 'info');
                };
            };
            reader.readAsDataURL(file);
        }

        function showSignatureCropper(file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                // Hide signature upload step and show cropper
                document.getElementById('signatureUploadStep').classList.add('hidden');
                document.getElementById('signatureCropperStep').classList.remove('hidden');

                // Set image source
                const cropSignatureImage = document.getElementById('cropSignatureImage');
                cropSignatureImage.src = e.target.result;

                // Initialize signature cropper
                if (signatureCropper) {
                    signatureCropper.destroy();
                }

                cropSignatureImage.onload = function () {
                    signatureCropper = new Cropper(cropSignatureImage, {
                        aspectRatio: 300 / 80, // 300x80 aspect ratio for signature
                        viewMode: 1,
                        dragMode: 'move',
                        autoCropArea: 0.9,
                        restore: false,
                        guides: true,
                        center: true,
                        highlight: false,
                        cropBoxMovable: true,
                        cropBoxResizable: true,
                        toggleDragModeOnDblclick: false,
                        minContainerWidth: 300,
                        minContainerHeight: 200,
                        preview: '#signatureCropperPreview'
                    });

                    showSignatureMessage('Position and resize the crop area to frame your signature perfectly', 'info');
                };
            };
            reader.readAsDataURL(file);
        }

        function cropPhoto() {
            if (!cropper) {
                showMessage('No image to crop', 'error');
                return;
            }

            showLoading(true);

            try {
                var canvas = cropper.getCroppedCanvas({
                    width: 300,
                    height: 300,
                    minWidth: 256,
                    minHeight: 256,
                    maxWidth: 4096,
                    maxHeight: 4096,
                    fillColor: '#fff',
                    imageSmoothingEnabled: true,
                    imageSmoothingQuality: 'high',
                });

                if (!canvas) {
                    showLoading(false);
                    showMessage('Failed to crop image. Please try again.', 'error');
                    return;
                }

                canvas.toBlob(function (blob) {
                    if (!blob) {
                        showLoading(false);
                        showMessage('Failed to process cropped image. Please try again.', 'error');
                        return;
                    }

                    croppedImageBlob = blob;

                    var previewImage = document.getElementById('croppedPreview');
                    previewImage.src = canvas.toDataURL('image/jpeg', 0.9);

                    // Show review step
                    document.getElementById('cropperStep').classList.add('hidden');
                    document.getElementById('reviewStep').classList.remove('hidden');

                    showLoading(false);
                    showMessage('Photo cropped successfully! Review the result below.', 'success');

                }, 'image/jpeg', 0.9);

            } catch (error) {
                showLoading(false);
                showMessage('Error cropping image: ' + error.message, 'error');
            }
        }

        function cropSignature() {
            if (!signatureCropper) {
                showSignatureMessage('No signature to crop', 'error');
                return;
            }

            showSignatureLoading(true);

            try {
                // Get cropped canvas for signature (300x80)
                const canvas = signatureCropper.getCroppedCanvas({
                    width: 300,
                    height: 80,
                    minWidth: 200,
                    minHeight: 50,
                    maxWidth: 4096,
                    maxHeight: 1024,
                    fillColor: '#fff',
                    imageSmoothingEnabled: true,
                    imageSmoothingQuality: 'high',
                });

                if (!canvas) {
                    showSignatureLoading(false);
                    showSignatureMessage('Failed to crop signature. Please try again.', 'error');
                    return;
                }

                // Convert to blob
                canvas.toBlob(function (blob) {
                    if (!blob) {
                        showSignatureLoading(false);
                        showSignatureMessage('Failed to process cropped signature. Please try again.', 'error');
                        return;
                    }

                    croppedSignatureBlob = blob;

                    // Show preview
                    const previewImage = document.getElementById('croppedSignaturePreview');
                    previewImage.src = canvas.toDataURL('image/jpeg', 0.9);

                    // Show review step
                    document.getElementById('signatureCropperStep').classList.add('hidden');
                    document.getElementById('signatureReviewStep').classList.remove('hidden');

                    showSignatureLoading(false);
                    showSignatureMessage('Signature cropped successfully! Review the result below.', 'success');

                }, 'image/jpeg', 0.9);

            } catch (error) {
                showSignatureLoading(false);
                showSignatureMessage('Error cropping signature: ' + error.message, 'error');
            }
        }

        function resetCropper() {
            if (cropper) {
                cropper.reset();
            }
        }

        function rotateCropper(degree) {
            if (cropper) {
                cropper.rotate(degree);
            }
        }

        function zoomCropper(ratio) {
            if (cropper) {
                cropper.zoom(ratio);
            }
        }

        // Signature cropper controls
        function resetSignatureCropper() {
            if (signatureCropper) {
                signatureCropper.reset();
            }
        }

        function rotateSignatureCropper(degree) {
            if (signatureCropper) {
                signatureCropper.rotate(degree);
            }
        }

        function zoomSignatureCropper(ratio) {
            if (signatureCropper) {
                signatureCropper.zoom(ratio);
            }
        }

        function startOver() {
            // Reset everything
            if (cropper) {
                cropper.destroy();
                cropper = null;
            }

            originalImageFile = null;
            croppedImageBlob = null;

            // Clear file input
            document.getElementById('<%= FileUploadPhoto.ClientID %>').value = '';

            // Reset to upload step
            document.getElementById('uploadStep').classList.remove('hidden');
            document.getElementById('cropperStep').classList.add('hidden');
            document.getElementById('reviewStep').classList.add('hidden');

            hideMessage();
        }

        function startOverSignature() {
            // Reset signature cropper
            if (signatureCropper) {
                signatureCropper.destroy();
                signatureCropper = null;
            }

            originalSignatureFile = null;
            croppedSignatureBlob = null;

            // Clear file input
            document.getElementById('<%= FileUploadSignature.ClientID %>').value = '';

            // Reset to upload step
            document.getElementById('signatureUploadStep').classList.remove('hidden');
            document.getElementById('signatureCropperStep').classList.add('hidden');
            document.getElementById('signatureReviewStep').classList.add('hidden');

            hideSignatureMessage();
        }

        function uploadCroppedPhoto() {
            if (!croppedImageBlob) {
                showMessage('No cropped photo available for upload. Please crop the image first.', 'error');
                return;
            }

            showMessage('Uploading cropped photo...', 'info');

            // Convert blob to base64
            const reader = new FileReader();
            reader.onloadend = function () {
                try {
                    const base64Data = reader.result.split(',')[1]; // Remove data:image/jpeg;base64, prefix

                    if (!base64Data || base64Data.length === 0) {
                        showMessage('Failed to process cropped photo for upload. Please try again.', 'error');
                        return;
                    }

                    // Set the hidden field with the cropped photo data
                    document.getElementById('<%= croppedImageBase64.ClientID %>').value = base64Data;

                    // Clear the original file input to ensure we don't upload the original
                    document.getElementById('<%= FileUploadPhoto.ClientID %>').value = '';

                    // Small delay to ensure the hidden field is set before postback
                    setTimeout(function () {
                        // Trigger server-side upload
                        document.getElementById('<%= btnUploadPhoto.ClientID %>').click();

                        // Force refresh the photo image after upload
                        setTimeout(function () {
                            try {
                                const photoImg = document.getElementById('<%= ImagePhoto.ClientID %>');
                                if (photoImg && photoImg.src) {
                                    const url = new URL(photoImg.src, window.location.origin);
                                    url.searchParams.set('v', Date.now());
                                    photoImg.src = url.toString();
                                }
                            } catch (error) {
                                console.log('Image refresh error:', error);
                            }
                        }, 1000); // Wait 1 second for server processing

                    }, 100);

                } catch (error) {
                    showMessage('Error preparing upload: ' + error.message, 'error');
                }
            };

            reader.onerror = function () {
                showMessage('Error reading cropped image for upload.', 'error');
            };

            reader.readAsDataURL(croppedImageBlob);
        }

        function uploadCroppedSignature() {
            if (!croppedSignatureBlob) {
                showSignatureMessage('No cropped signature available for upload. Please crop the signature first.', 'error');
                return;
            }

            showSignatureMessage('Uploading cropped signature...', 'info');

            // Convert blob to base64
            const reader = new FileReader();
            reader.onloadend = function () {
                try {
                    const base64Data = reader.result.split(',')[1]; // Remove data:image/jpeg;base64, prefix

                    if (!base64Data || base64Data.length === 0) {
                        showSignatureMessage('Failed to process cropped signature for upload. Please try again.', 'error');
                        return;
                    }

                    // Set the hidden field with the cropped signature data
                    document.getElementById('<%= croppedSignatureBase64.ClientID %>').value = base64Data;

                    // Clear the original file input to ensure we don't upload the original
                    document.getElementById('<%= FileUploadSignature.ClientID %>').value = '';

                    setTimeout(function () {
                        // Trigger server-side upload
                        document.getElementById('<%= btnUploadSignature.ClientID %>').click();

                        setTimeout(function () {
                            try {
                                const signatureImg = document.getElementById('<%= ImageSignature.ClientID %>');
                                if (signatureImg && signatureImg.src) {
                                    const url = new URL(signatureImg.src, window.location.origin);
                                    url.searchParams.set('v', Date.now());
                                    signatureImg.src = url.toString();
                                }
                            } catch (error) {
                                console.log('Image refresh error:', error);
                            }
                        }, 1000);

                    }, 100);

                } catch (error) {
                    showSignatureMessage('Error preparing upload: ' + error.message, 'error');
                }
            };

            reader.onerror = function () {
                showSignatureMessage('Error reading cropped signature for upload.', 'error');
            };

            reader.readAsDataURL(croppedSignatureBlob);
        }

        function showLoading(show) {
            const loading = document.getElementById('loadingSpinner');
            if (show) {
                loading.classList.add('show');
            } else {
                loading.classList.remove('show');
            }
        }

        function showSignatureLoading(show) {
            const loading = document.getElementById('signatureLoadingSpinner');
            if (show) {
                loading.classList.add('show');
            } else {
                loading.classList.remove('show');
            }
        }

        function showMessage(message, type) {
            const messageArea = document.getElementById('messageArea');
            if (messageArea) {
                messageArea.innerHTML = `<div class="message-${type}">${message}</div>`;
            }
        }

        function hideMessage() {
            const messageArea = document.getElementById('messageArea');
            if (messageArea) {
                messageArea.innerHTML = '';
            }
        }

        function showSignatureMessage(message, type) {
            const messageArea = document.getElementById('signatureMessageArea');
            if (messageArea) {
                messageArea.innerHTML = `<div class="message-${type}">${message}</div>`;
            }
        }

        function hideSignatureMessage() {
            const messageArea = document.getElementById('signatureMessageArea');
            if (messageArea) {
                messageArea.innerHTML = '';
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="application-container">
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
 <!-- Progress Navigation -->
                <div class="progress-nav">
                    <nav>
                        <ol class="breadcrumb-modern">
                            <%--<li class="active">Basic</li>--%>
                            <li >
                                <asp:HyperLink ID="hrefAppBasic" runat="server" data-step="1" data-label="Basic Info" title="Basic Info"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppPriority" runat="server" data-step="2" data-label="Program Priority" title="Program Priority"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppEducation" runat="server" data-step="3" data-label="Education" title="Education"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppRelation" runat="server" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppAddress" runat="server" data-step="5" data-label="Address" title="Address"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppAdditional" runat="server" data-step="6" data-label="Additional/Work" title="Additional/Work"></asp:HyperLink>
                            </li>
                            <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                            <li class="active">
                                <asp:HyperLink ID="hrefAppAttachment" runat="server" data-step="7" data-label="Upload Photo" title="Upload Photo"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppDeclaration" runat="server" data-step="8" data-label="Declaration" title="Declaration"></asp:HyperLink>
                            </li>
                        </ol>
                    </nav>
                </div>
                    <div class="panel-body">
                        <asp:Panel ID="messagePanel" runat="server">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            <br />
                        </asp:Panel>

                        <!-- Modern Guidelines Section -->
                        <div class="requirements-box">
                            <div class="requirement-header">
                                <i class="fas fa-exclamation-circle"></i>IMPORTANT REQUIREMENTS
                            </div>
                            <div class="requirement-content">
                                <div class="row">
                                    <div class="col-md-6 requirement-column photo-requirements">
                                        <h4><i class="fas fa-camera"></i>PHOTO</h4>
                                        <ul>
                                            <li><i class="fas fa-check-circle"></i>Passport size photo on solid background</li>
                                            <li><i class="fas fa-check-circle"></i>Size: 300×300 pixels</li>
                                            <li><i class="fas fa-check-circle"></i>Max 150KB (JPEG/JPG)</li>
                                        </ul>
                                    </div>
                                    <div class="col-md-6 requirement-column signature-requirements">
                                        <h4><i class="fas fa-signature"></i>SIGNATURE</h4>
                                        <ul>
                                            <li><i class="fas fa-check-circle"></i>Clear signature on white background</li>
                                            <li><i class="fas fa-check-circle"></i>Size: 300×80 pixels</li>
                                            <li><i class="fas fa-check-circle"></i>Max 150KB (JPEG/JPG)</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <!-- Photo Upload Section -->
                            <div class="col-md-6 required-upload">
                                <div class="upload-section">
                                    <div class="section-header">
                                        <i class="fas fa-camera"></i>Passport size photo <span class="required-badge">REQUIRED</span>
                                    </div>
                                    <div class="section-body">
                                        <!-- Message Area -->
                                        <div id="messageArea"></div>

                                        <!-- Step 1: Upload -->
                                        <div id="uploadStep">
                                            <div class="upload-area" id="photoUploadArea" title="Upload Photo">
                                                <div class="upload-icon"><i class="fas fa-cloud-upload-alt"></i></div>
                                                <div class="upload-text">Drop photo here or click to browse</div>
                                                <div class="upload-subtext">JPG - max 150KB</div>
                                                <div class="upload-feature">
                                                    <i class="fas fa-crop"></i>Crop and adjust your photo
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Step 2: Cropping -->
                                        <div id="cropperStep" class="hidden">
                                            <div class="cropper-container">
                                                <div class="cropper-wrapper">
                                                    <img id="cropImage" class="crop-image" alt="Image to crop">
                                                </div>

                                                <div class="cropper-preview-container">
                                                    <h5>Preview (300×300):</h5>
                                                    <div id="cropperPreview" class="cropper-preview"></div>
                                                </div>

                                                <div class="cropper-controls">
                                                    <button type="button" class="crop-btn crop-btn-secondary" onclick="resetCropper()">
                                                        <i class="fas fa-undo"></i>Reset
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="rotateCropper(-90)">
                                                        <i class="fas fa-undo"></i>Rotate Left
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="rotateCropper(90)">
                                                        <i class="fas fa-redo"></i>Rotate Right
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="zoomCropper(0.1)">
                                                        <i class="fas fa-search-plus"></i>Zoom In
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="zoomCropper(-0.1)">
                                                        <i class="fas fa-search-minus"></i>Zoom Out
                                                    </button>
                                                </div>

                                                <div style="text-align: center; margin-top: 20px;">
                                                    <button type="button" class="crop-btn crop-btn-danger" onclick="startOver()" style="margin-right: 10px;">
                                                        <i class="fas fa-times"></i>Cancel
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-success" onclick="cropPhoto()">
                                                        <i class="fas fa-check"></i>Crop Photo
                                                    </button>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Step 3: Review -->
                                        <div id="reviewStep" class="hidden">
                                            <div class="current-file">
                                                <div class="current-file-header">Cropped Photo:</div>
                                                <img id="croppedPreview" class="current-image" alt="Cropped photo">
                                                <div style="font-size: 12px; color: #666; margin-top: 8px;">
                                                    300×300 pixels | JPEG format
                                                </div>
                                            </div>

                                            <div style="text-align: center; margin-top: 15px;">
                                                <button type="button" class="btn btn-secondary" onclick="startOver()" style="margin-right: 10px;">
                                                    <i class="fas fa-redo"></i>Start Over
                                                </button>
                                                <button type="button" class="btn-upload-final" onclick="uploadCroppedPhoto()">
                                                    <i class="fas fa-check"></i>Upload This Photo
                                                </button>
                                            </div>
                                        </div>

                                        <!-- Loading Spinner -->
                                        <div class="loading-spinner" id="loadingSpinner">
                                            <div class="spinner"></div>
                                            <div class="spinner-text">Processing photo...</div>
                                            <div class="spinner-subtext">Please wait while we process your image</div>
                                        </div>

                                        <!-- Traditional Upload (Hidden) -->
                                        <div style="display: none;">
                                            <asp:FileUpload ID="FileUploadPhoto" runat="server" AllowMultiple="false" accept="image/*" />
                                            <asp:RegularExpressionValidator ID="rexvPhoto" runat="server" ControlToValidate="FileUploadPhoto"
                                                ErrorMessage="Only JPG and JPEG files are allowed" Display="Dynamic" ForeColor="Crimson"
                                                ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])$)">
                                            </asp:RegularExpressionValidator>
                                            <asp:Button ID="btnUploadPhoto" runat="server"
                                                Text="Upload Photo"
                                                Style="display: none"
                                                OnClick="btnUploadPhoto_Click" />
                                            <asp:HiddenField ID="croppedImageBase64" runat="server" />
                                        </div>

                                        <!-- Current Photo Display -->
                                        <div style="text-align: center; margin-top: 15px;">
                                            <div style="color: crimson; font-weight: bold;"><sup>File size: 150KB</sup></div>
                                            <asp:Image ID="ImagePhoto" runat="server"
                                                ImageUrl="~/Images/AppImg/user7.jpg"
                                                Width="154" Height="154" />
                                        </div>

                                        <asp:Panel ID="messagePanel_Photo" runat="server">
                                            <asp:Label ID="lblMessagePhoto" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>

                            <!-- Signature Upload Section -->
                            <div class="col-md-6 required-upload">
                                <div class="upload-section">
                                    <div class="section-header">
                                        <i class="fas fa-signature"></i>Signature <span class="required-badge">REQUIRED</span>
                                    </div>
                                    <div class="section-body">
                                        <!-- Signature Message Area -->
                                        <div id="signatureMessageArea"></div>

                                        <!-- Step 1: Upload Signature -->
                                        <div id="signatureUploadStep">
                                            <div class="upload-area" id="signatureUploadArea" title="Upload Signature">
                                                <div class="upload-icon"><i class="fas fa-cloud-upload-alt"></i></div>
                                                <div class="upload-text">Drop signature here or click to browse</div>
                                                <div class="upload-subtext">JPG - max 150KB</div>
                                                <div class="upload-feature">
                                                    <i class="fas fa-crop"></i>Crop and adjust your signature
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Step 2: Signature Cropping -->
                                        <div id="signatureCropperStep" class="hidden">
                                            <div class="cropper-container">
                                                <div class="cropper-wrapper">
                                                    <img id="cropSignatureImage" class="crop-image" alt="Signature to crop">
                                                </div>

                                                <div class="cropper-preview-container">
                                                    <h5>Preview (300×80):</h5>
                                                    <div id="signatureCropperPreview" class="cropper-preview signature-preview"></div>
                                                </div>

                                                <div class="cropper-controls">
                                                    <button type="button" class="crop-btn crop-btn-secondary" onclick="resetSignatureCropper()">
                                                        <i class="fas fa-undo"></i>Reset
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="rotateSignatureCropper(-90)">
                                                        <i class="fas fa-undo"></i>Rotate Left
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="rotateSignatureCropper(90)">
                                                        <i class="fas fa-redo"></i>Rotate Right
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="zoomSignatureCropper(0.1)">
                                                        <i class="fas fa-search-plus"></i>Zoom In
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-primary" onclick="zoomSignatureCropper(-0.1)">
                                                        <i class="fas fa-search-minus"></i>Zoom Out
                                                    </button>
                                                </div>

                                                <div style="text-align: center; margin-top: 20px;">
                                                    <button type="button" class="crop-btn crop-btn-danger" onclick="startOverSignature()" style="margin-right: 10px;">
                                                        <i class="fas fa-times"></i>Cancel
                                                    </button>
                                                    <button type="button" class="crop-btn crop-btn-success" onclick="cropSignature()">
                                                        <i class="fas fa-check"></i>Crop Signature
                                                    </button>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Step 3: Signature Review -->
                                        <div id="signatureReviewStep" class="hidden">
                                            <div class="current-file">
                                                <div class="current-file-header">Cropped Signature:</div>
                                                <img id="croppedSignaturePreview" class="current-signature" alt="Cropped signature">
                                                <div style="font-size: 12px; color: #666; margin-top: 8px;">
                                                    300×80 pixels | JPEG format
                                                </div>
                                            </div>

                                            <div style="text-align: center; margin-top: 15px;">
                                                <button type="button" class="btn btn-secondary" onclick="startOverSignature()" style="margin-right: 10px;">
                                                    <i class="fas fa-redo"></i>Start Over
                                                </button>
                                                <button type="button" class="btn-upload-final" onclick="uploadCroppedSignature()">
                                                    <i class="fas fa-check"></i>Upload This Signature
                                                </button>
                                            </div>
                                        </div>

                                        <!-- Signature Loading Spinner -->
                                        <div class="loading-spinner" id="signatureLoadingSpinner">
                                            <div class="spinner"></div>
                                            <div class="spinner-text">Processing signature...</div>
                                            <div class="spinner-subtext">Please wait while we process your signature</div>
                                        </div>

                                        <!-- Traditional Signature Upload (Hidden) -->
                                        <div style="display: none;">
                                            <asp:FileUpload ID="FileUploadSignature" runat="server" AllowMultiple="false" accept="image/*" />
                                            <asp:RegularExpressionValidator ID="rexvSignature" runat="server" ControlToValidate="FileUploadSignature"
                                                ErrorMessage="Only JPG and JPEG files are allowed" Display="Dynamic" ForeColor="Crimson"
                                                ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])$)">
                                            </asp:RegularExpressionValidator>
                                            <asp:Button ID="btnUploadSignature" runat="server"
                                                Text="Upload signature"
                                                Style="display: none"
                                                OnClick="btnUploadSignature_Click" />
                                            <asp:HiddenField ID="croppedSignatureBase64" runat="server" />
                                        </div>

                                        <!-- Current Signature Display -->
                                        <div style="text-align: center; margin-top: 15px;">
                                            <div style="color: crimson; font-weight: bold;"><sup>File size: 150KB</sup></div>
                                            <asp:Image ID="ImageSignature" runat="server"
                                                ImageUrl="~/Images/AppImg/sign2.png"
                                                Width="256" Height="128" />
                                        </div>

                                        <asp:Panel ID="messagePanel_Sign" runat="server">
                                            <asp:Label ID="lblMessageSign" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                            CssClass="btn btn-primary" />

                        <!-- Hidden test controls -->
                        <asp:FileUpload ID="File1" runat="server" AllowMultiple="false" accept="image/*" Visible="false" />
                        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" Visible="false" />
                        <asp:Label ID="lblmsg" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>