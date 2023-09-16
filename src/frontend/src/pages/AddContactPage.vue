<template>
  <BaseLayout>
    <template #toolbar>
      <h2>Add Contact</h2>
    </template>
    <template #default>
      <form @submit="onSubmit">
        <div class="card flex flex-column gap-2">
          <input type="hidden" v-bind="id" />
          <div class="p-inputgroup align-content-start" style="width: 30rem">
            <span class="p-inputgroup-addon">
              <i class="pi pi-user"></i>
            </span>
            <InputText
              placeholder="Name"
              v-bind="name"
              :class="{ 'p-invalid': errors.name }"
            />
          </div>
          <small class="p-error">
            {{ errors.name }}
          </small>

          <div class="p-inputgroup" style="width: 30rem">
            <span class="p-inputgroup-addon">
              <i class="pi pi-user"></i>
            </span>
            <InputText
              placeholder="Surname"
              v-bind="surname"
              :class="{ 'p-invalid': errors.surname }"
            />
          </div>
          <small class="p-error">
            {{ errors.surname }}
          </small>

          <div class="p-inputgroup" style="width: 30rem">
            <span class="p-inputgroup-addon">
              <i class="pi pi-envelope"></i
            ></span>
            <InputText
              placeholder="Email"
              type="email"
              v-bind="email"
              :class="{ 'p-invalid': errors.email }"
            />
          </div>
          <small class="p-error">
            {{ errors.email }}
          </small>

          <div class="p-inputgroup" style="width: 30rem">
            <span class="p-inputgroup-addon"> <i class="pi pi-phone"></i></span>
            <InputText
              placeholder="Cell Phone"
              v-bind="cellPhoneNumber"
              :class="{ 'p-invalid': errors.cellPhoneNumber }"
            />
          </div>
          <small class="p-error">
            {{ errors.cellPhoneNumber }}
          </small>

          <div class="p-inputgroup" style="width: 30rem">
            <span class="p-inputgroup-addon"> <i class="pi pi-phone"></i></span>
            <InputText
              placeholder="Home Phone"
              v-bind="homePhoneNumber"
              :class="{ 'p-invalid': errors.homePhoneNumber }"
            />
          </div>
          <small class="p-error">
            {{ errors.homePhoneNumber }}
          </small>

          <div class="p-inputgroup" style="width: 30rem">
            <Button type="submit" label="Save" style="margin-right: 0.5rem" />
            <Button type="button" label="Back" @click="goBack" />
          </div>
        </div>
      </form>
    </template>
  </BaseLayout>
</template>

<script lang="ts" setup>
import InputText from "primevue/inputtext";
import * as yup from "yup";
import BaseLayout from "@/layouts/BaseLayout.vue";
import Button from "primevue/button";
import router from "@/router";
import { Contact } from "@/models/contact";
import { useForm } from "vee-validate";
import { useContactsStore } from "@/store/contactsStore";
import { useToast } from "primevue/usetoast";
import { onBeforeMount, onMounted, ref } from "vue";
import { useRoute } from "vue-router";

const contactsStore = useContactsStore();
const contact = ref<Contact>();

const schema = yup.object({
  name: yup.string().required().label("Name"),
  surname: yup.string(),
  email: yup.string().email(),
  cellPhoneNumber: yup.string(),
  homePhoneNumber: yup.string(),
});

const { defineComponentBinds, handleSubmit, setFieldValue, errors } = useForm({
  validationSchema: schema,
});

const id = defineComponentBinds("id");
const name = defineComponentBinds("name");
const surname = defineComponentBinds("surname");
const email = defineComponentBinds("email");
const cellPhoneNumber = defineComponentBinds("cellPhoneNumber");
const homePhoneNumber = defineComponentBinds("homePhoneNumber");

const toast = useToast();
const editMode = ref(false);

onMounted(() => {
  const route = useRoute();
  const contactId = Number(route.params.id);
  if (contactId) {
    editMode.value = true;
    contact.value = contactsStore.contacts.find((c) => c.id == contactId);

    if (contact?.value) {
      setFieldValue("id", contact.value.id);
      setFieldValue("name", contact.value.name ?? "");
      setFieldValue("surname", contact.value.surname ?? "");
      setFieldValue("email", contact.value.email ?? "");
      setFieldValue("cellPhoneNumber", contact.value.cellPhoneNumber ?? "");
      setFieldValue("homePhoneNumber", contact.value.homePhoneNumber ?? "");
    }
  }
});

const onSubmit = handleSubmit(async (values: any) => {
  const newContact = {
    name: values.name,
    surname: values.surname,
    email: values.email,
    cellPhoneNumber: values.cellPhoneNumber,
    homePhoneNumber: values.homePhoneNumber,
  } as Contact;
  if(values?.id) {
    newContact.id = values.id;
  }
  try {
    if (!editMode.value) {
      await contactsStore.addContact(newContact);
      toast.add({
        severity: "info",
        summary: "Info",
        detail: "Contact Added",
        life: 3000,
        group: "msg",
      });
      router.push("/");
    } else {
      await contactsStore.editContact(newContact);
      toast.add({
        severity: "info",
        summary: "Info",
        detail: "Contact Edited",
        life: 3000,
        group: "msg",
      });
      router.push("/");
    }
  } catch (error) {
    console.log(error);
    toast.add({
      severity: "error",
      summary: "Error",
      detail: `Error occurred ${contactsStore?.errorMessage ?? error}`,
      life: 3000,
      group: "msg",
    });
  }
});

function goBack() {
  router.push("/");
}
</script>

<style lang="scss" scoped></style>
