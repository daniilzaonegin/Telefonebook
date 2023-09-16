<template>
  <BaseLayout>
    <h2>Please Register</h2>
    <form @submit="onSubmit">
      <div class="card flex flex-column gap-3">
        <div class="p-inputgroup align-content-start" style="width: 30rem">
          <span class="p-inputgroup-addon">
            <i class="pi pi-user"></i>
          </span>
          <InputText
            placeholder="Login"
            v-bind="login"
            :class="{ 'p-invalid': errors.login }"
          />
        </div>
        <small class="p-error">
          {{ errors.login }}
        </small>

        <div class="p-inputgroup" style="width: 30rem">
          <span class="p-inputgroup-addon">
            <i class="pi pi-lock"></i>
          </span>
          <InputText
            placeholder="Password"
            type="password"
            v-bind="password"
            :class="{ 'p-invalid': errors.password }"
          />
        </div>
        <small class="p-error">
          {{ errors.password }}
        </small>

        <div class="p-inputgroup" style="width: 30rem">
          <span class="p-inputgroup-addon">
            <i class="pi pi-lock"></i>
          </span>
          <InputText
            placeholder="RepeatPassword"
            type="password"
            v-bind="repeatPassword"
            :class="{ 'p-invalid': errors.repeatPassword }"
          />
        </div>
        <small class="p-error">
          {{ errors.repeatPassword }}
        </small>

        <div class="flex" style="width: 30rem">
          <Button type="submit" label="Register" icon="pi pi-check" style="margin-right: 0.5rem;" />
          <Button type="button" label="Back" icon="pi pi-times" severity="danger" @click="() => router.push('/')"/>
        </div>
      </div>
    </form>
  </BaseLayout>
</template>

<script lang="ts" setup>
import BaseLayout from '@/layouts/BaseLayout.vue';
import { useToast } from 'primevue/usetoast';
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import { useForm } from "vee-validate";
import * as yup from "yup";
import router from "@/router"
import { useUserDataStore } from '@/store/usersStore';
import contactsApiClient from "@/services/contacts-api-client";

const toast = useToast();
const userDataStore = useUserDataStore();

const schema = yup.object({
  login: yup.string().required().label("Login"),
  password: yup.string().required().label("Password"),
  repeatPassword: yup.string()
                     .required()
                     .oneOf([yup.ref("password")], 
                           "Passwords do not match")
});

const { defineComponentBinds, handleSubmit, setFieldValue, errors } = useForm({
  validationSchema: schema,
});
const login = defineComponentBinds("login");
const password = defineComponentBinds("password");
const repeatPassword = defineComponentBinds("repeatPassword");

const onSubmit = handleSubmit(async (values: any) => { 
  try {
    await contactsApiClient.register({
      username: values.login,
      providerKey: values.password,
    });
    router.push("/");
  } catch (error) {
    toast.add({
      severity: "error",
      summary: "Error",
      detail: `Register user failed. Error message: ${error}`,
      life: 3000,
      group: "msg",
    });
  }
});


</script>

<style lang="scss" scoped></style>
