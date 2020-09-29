/// <reference path="../vendor/jquery-validation/JqueryValidator.js" />
/// <reference path="../vendor/jquery-validation/JqueryValidator-AdditionalMethods.js" />
$(document).ready(function () {
    $.validator.addMethod('strongPassword', function (val, el) {
        return this.optional(el) || val.length >= 6 && /\a/.test(val) && /[a-z]/i.test(val);
    }, "Password must be of at least 6 characters and must contains at least one number and one character")


    $.validator.addMethod("phoneBN", function (value, element) {
        return this.optional(element) || /^(((\+|00)?880)|0)(\d){10}$/.test(value);
    }, "Please specify a valid BD phone number.");

    $('#inline_validation').validate({
        rules: {
            firstName: {
                required: true,
                lettersonly: true
            },
            lastName: {
                required: true,
                lettersonly: true
            },
            email: {
                required: true,
                email: true
            },
            userName: {
                required: true,
                alphanumeric: true
            },
            Mobile: {
                required: true,
                phoneBN: true
            },
            Website: {
                required: true,
                url: true
            },
            password: {
                required: true,
                strongPassword: true
            },
            confirmPassword: {
                required: true,
                equalTo: "#txtPassword"
            },
            Designation: {
                required: true
            },
            userType: {
                required: true
            },
            gender: {
                required: true
            },
            photo: {
                required: true
            }

        },
        messages: {
            firstName: {
                required: '<b class="text-danger">Required<b>'
            },
            lastName: {
                required: '<b class="text-danger">Required<b>'
            },
            email: {
                required: '<b class="text-danger">Required<b>',
                email: '<span class="text-danger">Invalid E-mail</span>'
            },
            userName: {
                required: '<b class="text-danger">Required<b>'
            },
            Mobile: {
                required: '<b class="text-danger">Required<b>',
            },
            Website: {
                required: '<b class="text-danger">Required<b>'
            },
            password: {
                required: '<b class="text-danger">Required<b>',
            },
            confirmPassword: {
                required: '<b class="text-danger">Required<b>',
                equalTo: '<span class="text-danger">Passowrd did not match</span>'
            },
            Designation: {
                required: '<b class="text-danger">Required<b>'
            },
            userType: {
                required: '<b class="text-danger">Required<b>'
            },
            gender: {
                required: true
            },
            photo: {
                required: true
            }
        }
    });
});