/*
 *  Copyright (c) 2015 The WebRTC project authors. All Rights Reserved.
 *
 *  Use of this source code is governed by a BSD-style license
 *  that can be found in the LICENSE file in the root of the source
 *  tree.
 */
'use strict';

// Put variables in global scope to make them available to the browser console.
const constraints = window.constraints = {
    audio: false,
    video: true
};

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/Web-rtc", {})
    //.withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
    .build();

function handleSuccess(stream) {
    const video = document.querySelector('video');
    const videoTracks = stream.getVideoTracks();
    console.log('Got stream with constraints:', constraints);
    console.log(`Using video device: ${videoTracks[0].label}`);
    window.stream = stream; // make variable available to browser console

    var arrayOfBlobs = [];
    var mediaRecorder = new MediaStreamRecorder(stream);
    mediaRecorder.mimeType = 'video/webm';
    mediaRecorder.ondataavailable = function (blob) {
        arrayOfBlobs.push(blob);
    };
    mediaRecorder.start(1000);

    (function looper() {
        // skip looper if recording has stopped
        if (!mediaRecorder) return;

        var sizeInBytes = new Blob(arrayOfBlobs).size;
        console.log('Blob size:', bytesToSize(sizeInBytes));

        // check after every 1-second
        setTimeout(looper, 1000);
    })();

    video.srcObject = stream;
}

function StreamToServer(obj) {
    connection.invoke("SendStream", obj);
}

function handleError(error) {
    if (error.name === 'ConstraintNotSatisfiedError') {
        let v = constraints.video;
        errorMsg(`The resolution ${v.width.exact}x${v.height.exact} px is not supported by your device.`);
    } else if (error.name === 'PermissionDeniedError') {
        errorMsg('Permissions have not been granted to use your camera and ' +
            'microphone, you need to allow the page access to your devices in ' +
            'order for the demo to work.');
    }
    errorMsg(`getUserMedia error: ${error.name}`, error);
}

function errorMsg(msg, error) {
    const errorElement = document.querySelector('#errorMsg');
    errorElement.innerHTML += `<p>${msg}</p>`;
    if (typeof error !== 'undefined') {
        console.error(error);
    }
}

function parseFile(file, callback) {

    var fileSize = file.size;
    var chunkSize = 64 * 1024; // bytes
    var offset = 0;
    var self = this; // we need a reference to the current object
    var chunkReaderBlock = null;

    var readEventHandler = function (evt) {

        if (evt.target.error === null) {
            offset += evt.target.result.length;
            //console.log(evt.target.result);
            callback(evt.target.result, offset, fileSize); // callback for handling read chunk

        } else {
            console.log("Read error: " + evt.target.error);
            return;
        }
        if (offset >= fileSize) {
            console.log("Done reading file");
            return;
        }

        // of to the next chunk
        chunkReaderBlock(offset, chunkSize, file);
    };

    chunkReaderBlock = function (_offset, length, _file) {

        var r = new FileReader();
        var blob = _file.slice(_offset, length + _offset);
        r.onload = readEventHandler;
        r.readAsDataURL(blob);
    };

    // now let's start the read with the first block
    chunkReaderBlock(offset, chunkSize, file);
}

async function init() {
    try {
        navigator.mediaDevices.getUserMedia(constraints).then(handleSuccess);
    } catch (e) {
        handleError(e);
    }
}

init();