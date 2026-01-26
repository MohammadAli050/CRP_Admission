<%@ Page Title="Application Form - Attachment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationAttachment.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationAttachment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">


    <!-- Cropper.js Library - A modern image cropping library -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/cropperjs/1.6.1/cropper.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/cropperjs/1.6.1/cropper.min.js"></script>

    <style>
        /* ===== BASE STYLES ===== */
        .application-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-lg);
            padding: 1rem;
            margin-bottom: 2rem;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
        }

        .progress-nav {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            padding: 1.5rem;
            margin-bottom: 2rem;
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

            .progress-nav .cd-breadcrumb {
                display: flex;
                align-items: center;
                justify-content: space-between;
                margin: 0;
                padding: 0;
                list-style: none;
                position: relative;
                z-index: 1;
            }

                .progress-nav .cd-breadcrumb::before {
                    content: '';
                    position: absolute;
                    top: 50%;
                    left: 0;
                    right: 0;
                    height: 3px;
                    background: linear-gradient(to right, #e2e8f0, var(--accent-light), #e2e8f0);
                    border-radius: 2px;
                    z-index: -1;
                }

                .progress-nav .cd-breadcrumb li {
                    position: relative;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    min-width: 60px;
                    min-height: 60px;
                    transition: var(--transition);
                    z-index: 2;
                }

                    .progress-nav .cd-breadcrumb li a {
                        position: relative;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        width: 60px;
                        height: 60px;
                        background: white;
                        border-radius: 50%;
                        border: 3px solid #e2e8f0;
                        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                        text-decoration: none;
                        transition: var(--transition);
                        cursor: pointer;
                        color: var(--gray);
                        font-weight: 700;
                        font-size: 0.85rem;
                    }

                        .progress-nav .cd-breadcrumb li a::before {
                            content: attr(data-step);
                            position: absolute;
                            top: 50%;
                            left: 50%;
                            transform: translate(-50%, -50%);
                            width: 30px;
                            height: 30px;
                            border-radius: 50%;
                            background: #e2e8f0;
                            color: var(--gray);
                            font-weight: 700;
                            font-size: 0.85rem;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            transition: var(--transition);
                        }

                        .progress-nav .cd-breadcrumb li a::after {
                            content: attr(data-label);
                            position: absolute;
                            top: 100%;
                            left: 50%;
                            transform: translateX(-50%);
                            color: var(--dark);
                            font-weight: 500;
                            font-size: 0.75rem;
                            white-space: nowrap;
                            margin-top: 0.5rem;
                            transition: var(--transition);
                            text-align: center;
                            max-width: 80px;
                            overflow: hidden;
                            text-overflow: ellipsis;
                        }

                    .progress-nav .cd-breadcrumb li.current a {
                        border-color: var(--accent);
                        transform: scale(1.1);
                        box-shadow: 0 6px 20px rgba(59, 130, 246, 0.3);
                    }

                        .progress-nav .cd-breadcrumb li.current a::before {
                            background: var(--accent);
                            color: white;
                        }

                        .progress-nav .cd-breadcrumb li.current a::after {
                            color: var(--accent);
                            font-weight: 600;
                        }

                    .progress-nav .cd-breadcrumb li.completed a {
                        border-color: var(--success);
                    }

                        .progress-nav .cd-breadcrumb li.completed a::before {
                            background: var(--success);
                            color: white;
                            content: '✓';
                            font-size: 1rem;
                        }

                    .progress-nav .cd-breadcrumb li:hover:not(.current) a {
                        transform: scale(1.05);
                        border-color: var(--accent-light);
                    }

                        .progress-nav .cd-breadcrumb li:hover:not(.current) a::before {
                            background: var(--accent-light);
                            color: white;
                        }

        #divProgress {
            display: none;
            z-index: 1000000;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            padding: 2rem;
            box-shadow: var(--shadow-xl);
            backdrop-filter: blur(10px);
        }

        /* Action buttons container */
        .action-buttons {
            text-align: center;
            padding: 2rem;
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            margin-top: 2rem;
        }

        /* Animation for form elements */
        .info-section {
            animation: fadeInUp 0.6s ease forwards;
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .abc {
                width: 100% !important;
            }

            .info-section {
                padding: 1.5rem;
            }

            .form-table td {
                padding: 0.5rem;
                display: block;
                width: 100% !important;
            }

                .form-table td:first-child {
                    width: 100% !important;
                    font-weight: 600;
                    border-bottom: none;
                    padding-bottom: 0.25rem;
                }

            .work-form-grid {
                grid-template-columns: 1fr;
            }

            .activity-row {
                grid-template-columns: 1fr;
            }

            .progress-nav {
                padding: 1rem;
                overflow-x: auto;
            }

                .progress-nav .cd-breadcrumb {
                    flex-wrap: nowrap;
                    gap: 0.5rem;
                    min-width: max-content;
                }

                    .progress-nav .cd-breadcrumb li {
                        min-width: 50px;
                        min-height: 50px;
                    }

                        .progress-nav .cd-breadcrumb li a {
                            width: 50px;
                            height: 50px;
                        }

                            .progress-nav .cd-breadcrumb li a::before {
                                width: 25px;
                                height: 25px;
                                font-size: 0.75rem;
                            }

                            .progress-nav .cd-breadcrumb li a::after {
                                font-size: 0.7rem;
                                max-width: 60px;
                            }
        }

        /* Helper message styling */
        .helper-message {
            background: linear-gradient(145deg, #dbeafe, #bfdbfe);
            color: var(--primary);
            padding: 1rem;
            border-radius: var(--radius-md);
            border-left: 4px solid var(--accent);
            margin-bottom: 1.5rem;
            font-size: 0.9rem;
        }

        /* Helper text styling */
        .helper-text {
            color: #0ea5e9;
            font-size: 0.8rem;
            display: block;
            margin-top: 0.25rem;
        }

        .panel {
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
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

        .feature {
            color: #3498db;
            font-weight: 600;
            margin-top: 15px;
            padding-top: 15px;
            border-top: 1px dashed #e0e0e0;
        }

            .feature i {
                color: #3498db !important;
            }

        /* ===== UPLOAD SECTIONS ===== */
        .required-upload {
            padding: 0 12px;
        }

        .upload-section {
            border: 2px solid #e74c3c;
            border-radius: 8px;
            margin-bottom: 25px;
            background: white;
            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
            overflow: hidden;
        }

        .section-header {
            background: linear-gradient(to right, #fef2f2, #fff);
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
                color: #e74c3c;
                font-size: 20px;
            }

        .required-badge {
            background: #e74c3c;
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
            border: 2px dashed #e74c3c;
            border-radius: 8px;
            padding: 30px 20px;
            text-align: center;
            background: rgba(231, 76, 60, 0.03);
            cursor: pointer;
            margin-bottom: 20px;
            transition: all 0.3s ease;
        }

            .upload-area:hover {
                background: rgba(231, 76, 60, 0.08);
                border-color: #c0392b;
                transform: translateY(-2px);
            }

        .upload-icon {
            font-size: 40px;
            color: #e74c3c;
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

        .cropper-preview {
            width: 150px;
            height: 150px;
            border: 2px solid #ddd;
            border-radius: 8px;
            margin: 0 auto;
            overflow: hidden;
            background: #f5f5f5;
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
        .btn-process {
            background: #3498db;
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

            .btn-process:hover {
                background: #2980b9;
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            }

            .btn-process i {
                margin-right: 8px;
            }

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
                box-shadow: 0 4px 8px rgba(0,0,0,0.1);
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
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }

        .current-signature {
            width: 280px;
            height: 120px;
            border: 1px solid #ddd;
            border-radius: 8px;
            object-fit: contain;
            background: white;
            padding: 15px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }

        .current-file-size {
            color: #e74c3c;
            font-size: 13px;
            font-weight: 600;
            margin-top: 10px;
        }

        /* ===== LOADING SPINNER ===== */
        .loading-spinner {
            display: none;
            text-align: center;
            padding: 25px;
            background: rgba(255,255,255,0.9);
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

        /* ===== SUBMIT SECTION ===== */
        .submit-section {
            text-align: center;
            margin-top: 35px;
            padding-top: 25px;
            border-top: 1px solid #eee;
        }

        .btn-submit {
            padding: 14px 45px;
            font-weight: 600;
            font-size: 16px;
            background: #27ae60;
            border: none;
            border-radius: 8px;
            transition: all 0.3s;
        }

            .btn-submit:hover {
                background: #219955;
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            }

        .submit-note {
            color: #e74c3c;
            font-size: 14px;
            margin-top: 15px;
            font-weight: 600;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .submit-note i {
                margin-right: 8px;
            }

        /* ===== HIDDEN CLASS ===== */
        .hidden {
            display: none !important;
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

            .btn-process,
            .btn-upload-final {
                padding: 10px 20px;
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

            .panel-title {
                font-size: 18px;
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

        /* ===== MESSAGE STYLES ===== */
        #messageArea {
            margin: 15px 0;
            min-height: 20px;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }

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
            backdrop-filter: blur(8px);
            -webkit-backdrop-filter: blur(8px);
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

        .message-warning {
            background: rgba(243, 156, 18, 0.1);
            color: #f39c12;
            border-left: 4px solid #f39c12;
        }

            .message-warning::before {
                content: "!";
                font-size: 16px;
                font-weight: 700;
                color: #f39c12;
                width: 20px;
                height: 20px;
                display: flex;
                align-items: center;
                justify-content: center;
                background: rgba(243, 156, 18, 0.15);
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

        @media (max-width: 768px) {
            .message-success,
            .message-error,
            .message-warning,
            .message-info {
                padding: 12px 16px;
                font-size: 13px;
                gap: 10px;
                margin-bottom: 8px;
            }

                .message-success::before,
                .message-error::before,
                .message-warning::before,
                .message-info::before {
                    width: 18px;
                    height: 18px;
                    font-size: 14px;
                }

                .message-error::before {
                    font-size: 18px;
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
                showMessage('Please upload JPG  image only', 'error');
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

                        // ✅ ADD THIS: Force refresh the photo image after upload
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

        function UploadPhoto(fileUpload) {
            if (fileUpload.value != '') {
                var fileName = fileUpload.files[0].name;
                var fileSize = fileUpload.files[0].size;
                var fileExtension = fileName.substring(fileName.lastIndexOf('.')).toLowerCase();
                var errorCount = 0;

                if (fileSize > 153600) { // 150KB
                    errorCount++;
                    alert("File size is over 150KB");
                    $('#MainContent_FileUploadPhoto').val('');
                }

                if (fileExtension != '.jpg' && fileExtension != '.jpeg') {
                    errorCount++;
                    alert("File Extension is not correct!");
                    $('#MainContent_FileUploadPhoto').val('');
                }

                if (errorCount == 0) {
                    const file = fileUpload.files[0];
                    if (file) {
                        processDroppedFile(file);
                    }
                }
            }
        }

        function UploadSignature(fileUpload) {
            if (fileUpload.value != '') {
                var fileName = fileUpload.files[0].name;
                var fileSize = fileUpload.files[0].size;
                var fileExtension = fileName.split('.')[fileName.split('.').length - 1].toLowerCase();
                var errorCount = 0;

                if (fileSize > 153600) { // 150KB
                    errorCount++;
                    alert("File size is over 150KB");
                    fileUpload.value = '';
                }

                if (fileExtension != 'jpg' && fileExtension != 'jpeg') {
                    errorCount++;
                    alert("File Extension is not correct!");
                    fileUpload.value = '';
                }

                if (errorCount == 0) {
                    // Instead of directly uploading, trigger the new signature cropper
                    const file = fileUpload.files[0];
                    if (file) {
                        processDroppedSignatureFile(file);
                    }
                }
            }
        }
    </script>
    <script>
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divProgress" style="display: none;">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="150px" Width="150px" />
        <div style="text-align: center; margin-top: 1rem; color: var(--primary); font-weight: 600;">
            Processing your information...
        </div>
    </div>
    <div class="application-container">
        <div class="progress-nav">
            <!-- Bachelor's Breadcrumb -->
            <div id="bachelorsBreadcrumb" runat="server">
                <nav>
                    <ol class="cd-breadcrumb">
                        <li>
                            <a href="ApplicationBasic.aspx" data-step="1" data-label="Basic" title="Basic"></a>
                        </li>
                        <li>
                            <a href="ApplicationEducation.aspx" data-step="2" data-label="Education" title="Education"></a>
                        </li>
                        <li>
                            <a href="ApplicationPriorityS.aspx" data-step="3" data-label="Priority" title="Priority"></a>
                        </li>
                        <li>
                            <a href="ApplicationRelation.aspx" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                        </li>
                        <li>
                            <a href="ApplicationAddress.aspx" data-step="5" data-label="Address" title="Address"></a>
                        </li>
                        <li>
                            <a href="ApplicationAdditional.aspx" data-step="5" data-label="Additional Info" title="Additional Info"></a>
                        </li>
                        <li class="current">
                            <a href="ApplicationAttachment.aspx" data-step="7" data-label="Photo" title="Photo"></a>
                        </li>
                        <li>
                            <a href="ApplicationDeclaration.aspx" data-step="8" data-label="Declaration" title="Declaration"></a>
                        </li>
                    </ol>
                </nav>
            </div>

            <!-- Master's Breadcrumb -->
            <div id="mastersBreadcrumb" runat="server">
                <nav>
                    <ol class="cd-breadcrumb">
                        <li>
                            <a href="ApplicationBasic.aspx" data-step="1" data-label="Basic Masters" title="Basic Masters"></a>
                        </li>
                        <li>
                            <a href="ApplicationEducation.aspx" data-step="2" data-label="Education" title="Education"></a>
                        </li>
                        <li>
                            <a href="ApplicationRelation.aspx" data-step="3" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                        </li>
                        <li>
                            <a href="ApplicationAddress.aspx" data-step="4" data-label="Address" title="Address"></a>
                        </li>
                        <li>
                            <a href="ApplicationAdditional.aspx" data-step="5" data-label="Additional Info" title="Additional Info"></a>
                        </li>
                        <li class="current">
                            <a href="ApplicationAttachment.aspx" data-step="6" data-label="Photo" title="Photo"></a>
                        </li>
                        <li>
                            <a href="ApplicationDeclaration.aspx" data-step="7" data-label="Declaration" title="Declaration"></a>
                        </li>
                    </ol>
                </nav>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">

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
                            <!-- Photo Upload Section - ALL ORIGINAL ASP CONTROLS PRESERVED -->
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
                                                <div style="font-size: 24px; margin-bottom: 10px;"><i class="fas fa-cloud-upload-alt"></i></div>
                                                <div style="font-weight: bold;">Drop photo here or click to browse</div>
                                                <div style="color: #666; font-size: 12px; margin-top: 5px;">JPG - max 150KB</div>
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

                                        <!-- Traditional Upload-->
                                        <div style="display: none;">
                                            <asp:FileUpload ID="FileUploadPhoto" runat="server" AllowMultiple="false" accept="image/*" />
                                            <asp:RegularExpressionValidator ID="rexvPhoto" runat="server" ControlToValidate="FileUploadPhoto"
                                                ErrorMessage="Only .jpg, , and .jpeg" Display="Dynamic" ForeColor="Crimson"
                                                ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Jj][Pp][Ee][Gg])$)"></asp:RegularExpressionValidator>
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

                            <!-- Signature Upload Section - ALL ORIGINAL ASP CONTROLS PRESERVED -->
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
                                                <div style="font-size: 24px; margin-bottom: 10px;"><i class="fas fa-cloud-upload-alt"></i></div>
                                                <div style="font-weight: bold;">Drop signature here or click to browse</div>
                                                <div style="color: #666; font-size: 12px; margin-top: 5px;">JPG - max 150KB</div>
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
                                                    <div id="signatureCropperPreview" class="cropper-preview" style="width: 300px; height: 80px;"></div>
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

                                        <!-- Traditional Signature Upload (Preserved exactly as original) -->
                                        <div style="display: none;">
                                            <asp:FileUpload ID="FileUploadSignature" runat="server" AllowMultiple="false" accept="image/*" />
                                            <asp:RegularExpressionValidator ID="rexvSignature" runat="server" ControlToValidate="FileUploadSignature"
                                                ErrorMessage="Only .jpg, and .jpeg" Display="Dynamic" ForeColor="Crimson"
                                                ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Jj][Pp][Ee][Gg])$)"></asp:RegularExpressionValidator>
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

                        <!-- Submit Section -->
                        <div class="submit-section">
                            <asp:Button ID="btnNext" runat="server" Text="Next"
                                CssClass="btn btn-primary btn-submit" OnClick="btnNext_Click" />
                            <div class="submit-note">
                                <i class="fas fa-exclamation-triangle"></i>Please upload both photo and signature before submitting
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <%-- end row --%>
</asp:Content>
