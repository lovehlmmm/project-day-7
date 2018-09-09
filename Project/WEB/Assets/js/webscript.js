function openAttachment() {
  document.getElementById('attachment').click();
}

function fileSelected(input){
  document.getElementById('btnAttachment').value = "File: " + input.files[0].name
}

function previewImages() {

  var $preview = $('#preview').empty();
  if (this.files) $.each(this.files, readAndPreview);

  function readAndPreview(i, file) {
    
    if (!/\.(jpe?g|png|gif)$/i.test(file.name)){
      return alert(file.name +" is not an image");
    } // else...
    
    var reader = new FileReader();

    $(reader).on("load", function() {
      $preview.append($("<img/>", {src:this.result, height:200}));
    });

    reader.readAsDataURL(file);
    
  }
}

$('#attachment').on("change", previewImages);