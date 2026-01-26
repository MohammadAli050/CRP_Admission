<%@ Page Title="EncDec" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EncDec.aspx.cs" Inherits="Admission.Admission.Admin.EncDec" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <u>Encrypt</u><br />
    <br />
    Original Text:
    <asp:TextBox ID="txtOriginalText" runat="server" Text="" Width="500"/>
    <br />
    <br />
    Encrypted Text:
    <asp:Label ID="lblEncryptedText" runat="server" Text="" />
    <br />
    <br />
    <asp:Button ID="btnEncrypt" OnClick="btnEncrypt_Click" Text="Encrypt" runat="server" />
    <hr />
    
    <u>Decrypt</u>
    <br />
    <br />
    Encrypted Text:
    <asp:TextBox ID="txtEncryptedText" runat="server" Text=""  Width="500"/>
    <br />
    <br />
    Decrypted Text:
    <asp:Label ID="lblDecryptedText" runat="server" Text="" />
    <br />
    <br />
    <asp:Button ID="btnDecrypt" OnClick="btnDecrypt_Click" Text="Decrypt" runat="server" />


</asp:Content>
